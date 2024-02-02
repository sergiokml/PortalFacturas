using Cve.Impuestos.Connected_Services;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Cve.Impuestos.Services.DteFirma
{
    internal class FirmaDteService : WcfSemillaToken, IFirmaDteService
    {
        private readonly IRepositoryBaseRest repo;
        private readonly string path;
        private readonly string pass;
        private readonly string rut;

        public FirmaDteService(IRepositoryBaseRest repo, IConfiguration config)
            : base(config)
        {
            this.repo = repo;
            path = config.GetSection("Certificado:Path").Value!;
            pass = config.GetSection("Certificado:Password").Value!;
            rut = config.GetSection("Certificado:Rut").Value!;
        }

        public async Task<XDocument> EnviarCurl(string rutemisor, string dvemisor, string namefile)
        {
            // PROG 1.0;
            await GetToken();
            // Token = "ZSQY00G15NGEJ";
            string curl = string.Format(
                "curl -X POST \"{0}\" "
                    + "-H \"User-Agent: Mozilla/4.0 (compatible; PROG 1.0; Windows NT)\" "
                    + "-H \"Cookie: TOKEN={1}\" "
                    + "-H \"Content-Type: multipart/form-data\" "
                    + "-F \"rutSender={2}\" "
                    + "-F \"dvSender={3}\" "
                    + "-F \"rutCompany={4}\" "
                    + "-F \"dvCompany={5}\" "
                    + "-F \"archivo=@{6}\"",
                "https://palena.sii.cl/cgi_dte/UPL/DTEUpload",
                Token,
                rut!.Split('-')!.GetValue(0)!.ToString()!,
                rut!.Split('-')!.GetValue(1)!.ToString()!,
                rutemisor,
                dvemisor,
                $"{Environment.CurrentDirectory}\\{namefile}"
            );
            string rr = await repo.ExecuteCurl(curl);
            XDocument res = XDocument.Parse(rr);
            return res;
        }

        public async Task<XDocument> EnviarHttp(string rutemisor, string dvemisor, string namefile)
        {
            await GetToken();
            HttpResponseMessage? msg = await repo.SendAsync2(
                Token!,
                namefile,
                rut!.Split('-')!.GetValue(0)!.ToString()!,
                rut!.Split('-')!.GetValue(1)!.ToString()!,
                rutemisor,
                dvemisor
            )!;
            await EnsureSuccessStatus(msg);
            string r = await msg.Content.ReadAsStringAsync();
            XDocument res = XDocument.Parse(r);
            return res;
        }

        public override async Task EnsureSuccessStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await GetToken();
                }
                else
                {
                    throw new Exception(
                        $"{response.StatusCode}:{response.RequestMessage!.RequestUri}"
                    );
                }
            }
        }

        public async Task<string> FirmarTradicional(
            List<Tuple<XDocument, string>> docs,
            string idset,
            string rutemisor,
            string rutreceptor,
            string fecharesol,
            string nroresol,
            string tipodte
        )
        {
            string signingtime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            List<string> dtelist = new();
            foreach (Tuple<XDocument, string> item in docs)
            {
                // ID
                string Id = item!.Item1.Root!.Element("Documento")!.FirstAttribute!.Value;
                //
                XDocument xdoc = item!.Item1;
                RemoverTagSoftland(xdoc);
                string xmlStr = xdoc.ToString();
                //
                xmlStr = xmlStr.Replace("@!TIMESTAMP!@", signingtime);
                xmlStr = xmlStr.Replace("@!FECHA!@", signingtime[..10]);
                // CAF
                Match m = Regex.Match(item.Item2, @"(<CAF.*?</CAF>)", RegexOptions.Singleline);
                xmlStr = xmlStr.Replace("<CAF version=\"1.0\">@!CAF!@</CAF>", m.Value);
                // CREAR FRMT CON "DD"
                Match match = Regex.Match(xmlStr, @"(<DD.*?</DD>)", RegexOptions.Singleline);
                if (match.Success)
                {
                    SHA1 sha1Hasher = SHA1.Create();
                    byte[] data = sha1Hasher.ComputeHash(Encoding.Latin1.GetBytes(match.Value));
                    string frmtSig = Convert.ToBase64String(data);
                    xmlStr = xmlStr.Replace("@!FRMT-SIG!@", frmtSig);
                }
                // AGREGAR SIGNATURE
                string ddd = FirmarXml(xmlStr, path, pass, Id);
                dtelist.Add(ddd);
            }
            string xmlsetdte = SobreTemplate(
                idset,
                rutemisor,
                rutreceptor,
                fecharesol,
                nroresol,
                tipodte
            );
            xmlsetdte = xmlsetdte.Replace("@!TIMESTAMP!@", signingtime);
            xmlsetdte = xmlsetdte.Replace("@!NUM-DTE!@", docs.Count.ToString());
            string toinsert = string.Join("\n", dtelist);
            xmlsetdte = xmlsetdte.Replace("<DTE>@!SET-OF-DTE!@</DTE>", toinsert);
            if (!xmlsetdte.Contains("@!"))
            {
                string nombrearchivo = "outputxmlFile100.xml";
                string firmado = FirmarXml(xmlsetdte, path, pass, idset);
                await File.WriteAllTextAsync(
                    nombrearchivo,
                    firmado,
                    Encoding.GetEncoding("iso-8859-1")
                );
                //if (Validar(nombrearchivo, ""))
                //{
                return nombrearchivo;
                //}
            }
            return null!;
        }

        public string FirmarXml(string documento, string path, string pass, string id)
        {
            byte[] rawCert = File.ReadAllBytes(path);
            X509Certificate2 x509 = new(rawCert, pass);
            RSA? privateKey = x509.GetRSAPrivateKey();
            try
            {
                XmlDocument doc = new() { PreserveWhitespace = true };
                doc.LoadXml(documento);
                SignedXml signedXml = new(doc) { SigningKey = privateKey };
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
                Reference reference = new($"#{id}") { DigestMethod = SignedXml.XmlDsigSHA1Url };
                Signature XMLSignature = signedXml.Signature;
                XMLSignature.SignedInfo.AddReference(reference);
                KeyInfo keyInfo = new();
                keyInfo.AddClause(new RSAKeyValue(privateKey));
                keyInfo.AddClause(new KeyInfoX509Data(x509));
                XMLSignature.KeyInfo = keyInfo;
                signedXml.ComputeSignature();
                XmlElement xmlDigitalSignature = signedXml.GetXml();
                _ = doc.DocumentElement!.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                if (signedXml.CheckSignature())
                {
                    Console.WriteLine($"Firmando... :{id}");
                    return doc.InnerXml;
                }
                return null!;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public string SobreTemplate(
            string idset,
            string rutenvia,
            string rutreceptor,
            string fecharesol,
            string nroresol,
            string tipodte
        )
        {
            string s =
                @$"<?xml version=""1.0"" encoding=""ISO-8859-1""?>
<EnvioDTE xmlns=""http://www.sii.cl/SiiDte""
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
xsi:schemaLocation=""http://www.sii.cl/SiiDte EnvioDTE_v10.xsd"" 
version=""1.0"">
<SetDTE ID=""{idset}"">
<Caratula version=""1.0"">
<RutEmisor>{rutenvia}</RutEmisor>
<RutEnvia>{rut}</RutEnvia>
<RutReceptor>{rutreceptor}</RutReceptor>
<FchResol>{fecharesol}</FchResol>
<NroResol>{nroresol}</NroResol>
<TmstFirmaEnv>@!TIMESTAMP!@</TmstFirmaEnv>
<SubTotDTE>
<TpoDTE>{tipodte}</TpoDTE>
<NroDTE>@!NUM-DTE!@</NroDTE>
</SubTotDTE>
</Caratula>
<DTE>@!SET-OF-DTE!@</DTE>
</SetDTE>
</EnvioDTE>";
            return s;
        }

        public static void RemoverTagSoftland(XDocument doc)
        {
            // SET FE : DEBE SER IGUAL A LA FECHA DE EMISIÓN DEL DTE
            doc.Root!.Element("Documento")!.Element("TED")!.Element("DD")!.Element("FE")!.Value =
                "@!FECHA!@";
            // SET TSTED
            doc.Root!
                .Element("Documento")!
                .Element("TED")!
                .Element("DD")!
                .Element("TSTED")!
                .Value = "@!TIMESTAMP!@";
            // SET CAF
            doc.Root!
                .Element("Documento")!
                .Element("TED")!
                .Element("DD")!
                .Element("CAF")!
                //.Element("DA")!
                //.Element("RSAPK")!
                .Value = "@!CAF!@";

            // SET FRMT
            doc.Root!.Element("Documento")!.Element("TED")!.Element("FRMT")!.Value = "@!FRMT-SIG!@";
            // SET TmstFirma
            doc.Root!.Element("Documento")!.Element("TmstFirma")!.Value = "@!TIMESTAMP!@";
            // REMOVE SIGNATURE
            //doc!.Root!.Elements().Where(c => c.Name.LocalName == "Signature")!.First().Value =
            //    "@!SIGNATURE!@";
            doc!.Root!.Elements().Where(c => c.Name.LocalName == "Signature")!.First().Remove();
        }
        //private void test()
        //{
        //    using SoftlandContext? dbContext = dbsoft.CreateDbContext("ALFA");
        //    DteDoccab? dte = dbContext.DteDoccabs
        //        .Where(c => c.Folio == 3325)
        //        .Include(c => c.DteDocdets)
        //        .Include(c => c.DteDocrefs)
        //        .FirstOrDefault();
        //    var aux = dbContext.GetAuxiliar(dte!.CdgIntRecep!).Result;
        //    //sii.FirmaDteService.Test();
        //    var xmldte = new DTEDefType { version = 1 };
        //    var dtestring = new DTEDefTypeDocumento
        //    {
        //        Encabezado = new DTEDefTypeDocumentoEncabezado()
        //        {
        //            IdDoc = new DTEDefTypeDocumentoEncabezadoIdDoc()
        //            {
        //                TipoDTE = (DTEType)dte!.TipoDte,
        //                Folio = dte.Folio.ToString(),
        //                FchEmis = dte.FchEmis!.Value,
        //                FmaPago = DTEDefTypeDocumentoEncabezadoIdDocFmaPago.Item2,
        //                TermPagoGlosa = "CREDITO",
        //                FchVenc = dte.FchVenc!.Value,
        //            },
        //            Emisor = new DTEDefTypeDocumentoEncabezadoEmisor()
        //            {
        //                RUTEmisor = dte.Rutemisor,
        //                RznSoc = dte.RznSoc,
        //                GiroEmis = dte.GiroEmis,
        //                Acteco = new string[] { dte.Acteco01.ToString()! },
        //                DirOrigen = dte.DirOrigen,
        //                CmnaOrigen = dte.CmnaOrigen,
        //                CiudadOrigen = dte.CiudadOrigen,
        //                CdgVendedor = dte.CdgVendedor
        //            },
        //            Receptor = new DTEDefTypeDocumentoEncabezadoReceptor()
        //            {
        //                RUTRecep = dte.Rutrecep,
        //                CdgIntRecep = dte.CdgIntRecep,
        //                RznSocRecep = dte.RznSocRecep,
        //                GiroRecep = dte.GiroRecep,
        //                Contacto = dte.Contacto,
        //                CorreoRecep = aux.EMailDte,
        //                DirRecep = dte.DirRecep,
        //                CmnaRecep = dte.CmnaRecep
        //            },
        //            Totales = new DTEDefTypeDocumentoEncabezadoTotales()
        //            {
        //                MntNeto = dte.MntNeto.ToString(),
        //                TasaIVA = Convert.ToDecimal(dte.TasaIva),
        //                IVA = dte.Iva.ToString(),
        //                MntTotal = dte.MntTotal.ToString()
        //            }
        //        },
        //        Detalle = new DTEDefTypeDocumentoDetalle[dte.DteDocdets.Count],
        //        Referencia = new DTEDefTypeDocumentoReferencia[dte.DteDocrefs.Count]
        //    };
        //    for (int i = 0; i < dte.DteDocdets.Count; i++)
        //    {
        //        dbContext
        //            .Entry(dte.DteDocdets.ElementAt(i))
        //            .Collection(c => c.DteDocdetcods)
        //            .Load();
        //        var detalle = new DTEDefTypeDocumentoDetalle()
        //        {
        //            NroLinDet = dte.DteDocdets.ElementAt(i).NroLinDet.ToString()
        //        };
        //        var cod = dte.DteDocdets.ElementAt(i).DteDocdetcods.First();
        //        var a = new DTEDefTypeDocumentoDetalleCdgItem
        //        {
        //            TpoCodigo = cod.TpoCodigo.ToString(),
        //            VlrCodigo = cod.VlrCodigo
        //        };
        //        detalle.CdgItem = new DTEDefTypeDocumentoDetalleCdgItem[] { a };
        //        detalle.NmbItem = dte.DteDocdets.ElementAt(i).NmbItem;
        //        detalle.QtyItem = Convert.ToDecimal(dte.DteDocdets.ElementAt(i).QtyItem);
        //        detalle.UnmdItem = dte.DteDocdets.ElementAt(i).UnmdItem;
        //        detalle.PrcItem = Convert.ToDecimal(dte.DteDocdets.ElementAt(i).PrcItem);
        //        detalle.MontoItem = dte.DteDocdets.ElementAt(i).MontoItem.ToString();
        //        dtestring.Detalle.SetValue(detalle, i);
        //    }

        //    // REFERENCIAS
        //    for (int i = 0; i < dte.DteDocrefs.Count; i++)
        //    {
        //        var reff = new DTEDefTypeDocumentoReferencia
        //        {
        //            NroLinRef = dte.DteDocrefs.ElementAt(i).NroLinRef.ToString(),
        //            TpoDocRef = dte.DteDocrefs.ElementAt(i).TpoDocRef,
        //            FolioRef = dte.DteDocrefs.ElementAt(i).FolioRef,
        //            FchRef = dte.DteDocrefs.ElementAt(i).FchRef!.Value,
        //            RazonRef = dte.DteDocrefs.ElementAt(i).RazonRef
        //        };
        //        dtestring.Referencia.SetValue(reff, i);
        //    }

        //    // TED
        //    var caf = dbContext.DteSiicafs
        //        .Where(c => c.FolioD <= dte.Folio && c.FolioH >= dte.Folio)
        //        .FirstOrDefault();
        //    AUTORIZACION2 add;
        //    using (XmlReader reader = XmlReader.Create(new StringReader(caf.Cafxml)))
        //    {
        //        XmlSerializer serializer =
        //            new(typeof(AUTORIZACION2), new XmlRootAttribute("AUTORIZACION"));
        //        add = (AUTORIZACION2)serializer.Deserialize(reader)!;
        //    }
        //    var ted = new DTEDefTypeDocumentoTED
        //    {
        //        DD = new DTEDefTypeDocumentoTEDDD()
        //        {
        //            RE = dte.Rutemisor,
        //            TD = DTEType.Item33,
        //            F = dte.Folio.ToString(),
        //            //FE = "@!FECHA!@"
        //            RR = dte.Rutrecep,
        //            RSR = dte.RznSocRecep,
        //            MNT = Convert.ToUInt64(dte.MntTotal),
        //            IT1 = dte.DteDocdets.ToList()[0].NmbItem,
        //            CAF = add.CAF
        //        }
        //    };
        //    dtestring.TED = ted;
        //}
    }
}

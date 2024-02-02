using System.Net.Http.Json;
using System.Text.Json;

using Cve.Impuestos.Connected_Services;
using Cve.Impuestos.Helpers;
using Cve.Impuestos.Infraestructure;
using Cve.Impuestos.Models;
using Cve.Impuestos.Serializadores;
using Cve.Impuestos.Services.Interfaces;

using Microsoft.Extensions.Configuration;

using static Cve.Impuestos.Serializadores.RespListarEventosHistDoc;

namespace Cve.Impuestos.Services
{
    internal class RegValidaDteService : WcfSemillaToken, IRegValidaDteService
    {
        private const string receptor =
            "cl.sii.sdi.lob.diii.registrorechazodtej6.data.api.interfaces.FacadeService/validarAccesoReceptor";
        private readonly IRepositoryBaseRest repo;
        private readonly string path;
        private readonly string pass;
        private readonly string rut;

        public RegValidaDteService(IRepositoryBaseRest repo, IConfiguration config)
            : base(config)
        {
            this.repo = repo;
            path = config.GetSection("Certificado:Path").Value!;
            pass = config.GetSection("Certificado:Password").Value!;
            rut = config.GetSection("Certificado:Rut").Value!;
        }

        public async Task<RegValidaDteResp?> ConsultaEstadoDte(
            string rutemisor,
            string dv,
            string folio,
            string tipo
        )
        {
            MetaDataRegValidaDteReqModel meta = new() { Namespace = receptor, TransactionId = "0" };
            DataRegValidaDteReqModel data =
                new()
                {
                    RutEmisor = rutemisor!,
                    DvEmisor = dv,
                    TipoDoc = tipo,
                    Folio = folio
                };
            await GetToken();
            data.RutToken = rut.Split("-").GetValue(0)!.ToString();
            data.DvToken = rut.Split("-").GetValue(1)!.ToString();
            meta.ConversationId = Token;
            RegValidaDteReq? dte = new(meta, data);
            string? json = JsonSerializer.Serialize(dte);
            HttpResponseMessage? msg = await repo.PostJson(
                json,
                Properties.Impuestos.UrlValidaDte,
                Token!
            )!;
            await EnsureSuccessStatus(msg);
            return await msg.Content.ReadFromJsonAsync<RegValidaDteResp>();
        }

        public async Task<RespIngresarAceptacionReclamoDoc.@return> IngresaAceptacionReclamo(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio,
            string accionDoc
        )
        {
            // RCD - ACD
            if (Token == null)
            {
                await GetToken();
            }
            HttpResponseMessage? msg = await repo.SendSoap(
                new(
                    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                        + "xmlns:ws=\"http://ws.registroreclamodte.diii.sdi.sii.cl\">\r\n"
                        + "<soapenv:Header/>\r\n"
                        + "<soapenv:Body>\r\n"
                        + "<ws:ingresarAceptacionReclamoDoc>\r\n"
                        + $"<rutEmisor>{rutEmisor}</rutEmisor>\r\n"
                        + $"<dvEmisor>{dvEmisor}</dvEmisor>\r\n"
                        + $"<tipoDoc>{tipoDoc}</tipoDoc>\r\n"
                        + $"<folio>{folio}</folio>\r\n"
                        + $"<accionDoc>{accionDoc}</accionDoc>\r\n"
                        + "</ws:ingresarAceptacionReclamoDoc>\r\n"
                        + "</soapenv:Body>\r\n"
                        + "</soapenv:Envelope>",
                    null,
                    "application/xml"
                ),
                HttpMethod.Post,
                Properties.Impuestos.WsdlRegistroReclamoDte,
                Token!
            )!;
            await EnsureSuccessStatus(msg);
            RespIngresarAceptacionReclamoDoc.Envelope res =
                XsltHelper.DeserializeStream<RespIngresarAceptacionReclamoDoc.Envelope>(
                    await msg.Content.ReadAsStreamAsync()
                );
            return res.Body.ingresarAceptacionReclamoDocResponse!.@return;
        }

        // NO ME SIRVE POR EL MOMENTO...
        //public async Task<RESPUESTA> ConsultaEstadoDteAv(
        //    string RutEmpresa,
        //    string DvEmpresa,
        //    string RutReceptor,
        //    string DvReceptor,
        //    string TipoDte,
        //    string FolioDte,
        //    string FechaEmisionDte,
        //    string MontoDte,
        //    string FirmaDte
        //)
        //{
        //    await GetToken();
        //    RESPUESTA resp = await ConsultaAvanzadaEstadoDte(
        //        RutEmpresa,
        //        DvEmpresa,
        //        RutReceptor,
        //        DvReceptor,
        //        TipoDte,
        //        FolioDte,
        //        FechaEmisionDte,
        //        MontoDte,
        //        FirmaDte
        //    );

        //    return resp;
        //}


        public async Task<@return?> listarEventosHistDoc(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio
        )
        {
            if (Token == null)
            {
                await GetToken();
            }
            // \r\n is Windows. => Environment.NewLine
            HttpResponseMessage? msg = await repo.SendSoap(
                new(
                    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                        + "xmlns:ws=\"http://ws.registroreclamodte.diii.sdi.sii.cl\">\r\n"
                        + "<soapenv:Header/>\r\n"
                        + "<soapenv:Body>\r\n"
                        + "<ws:listarEventosHistDoc>\r\n"
                        + $"<rutEmisor>{rutEmisor}</rutEmisor>\r\n"
                        + $"<dvEmisor>{dvEmisor}</dvEmisor>\r\n"
                        + $"<tipoDoc>{tipoDoc}</tipoDoc>\r\n"
                        + $"<folio>{folio}</folio>\r\n"
                        + "</ws:listarEventosHistDoc>\r\n"
                        + "</soapenv:Body>\r\n"
                        + "</soapenv:Envelope>",
                    null,
                    "application/xml"
                ),
                HttpMethod.Post,
                Properties.Impuestos.WsdlRegistroReclamoDte,
                Token!
            )!;
            await EnsureSuccessStatus(msg);
            Envelope res = XsltHelper.DeserializeStream<RespListarEventosHistDoc.Envelope>(
                await msg.Content.ReadAsStreamAsync()
            );
            return res.Body!.listarEventosHistDocResponse!.@return;
        }

        public async Task<string?> ConsultarFechaRecepcionSii(
            string rutEmisor,
            string dvEmisor,
            string tipoDoc,
            string folio
        )
        {
            if (Token == null)
            {
                await GetToken();
            }
            // \r\n is Windows. => Environment.NewLine
            HttpResponseMessage? msg = await repo.SendSoap(
                new(
                    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" "
                        + "xmlns:ws=\"http://ws.registroreclamodte.diii.sdi.sii.cl\">\r\n"
                        + "<soapenv:Header/>\r\n"
                        + "<soapenv:Body>\r\n"
                        + "<ws:consultarFechaRecepcionSii>\r\n"
                        + $"<rutEmisor>{rutEmisor}</rutEmisor>\r\n"
                        + $"<dvEmisor>{dvEmisor}</dvEmisor>\r\n"
                        + $"<tipoDoc>{tipoDoc}</tipoDoc>\r\n"
                        + $"<folio>{folio}</folio>\r\n"
                        + "</ws:consultarFechaRecepcionSii>\r\n"
                        + "</soapenv:Body>\r\n"
                        + "</soapenv:Envelope>",
                    null,
                    "application/xml"
                ),
                HttpMethod.Post,
                Properties.Impuestos.WsdlRegistroReclamoDte,
                Token!
            )!;
            await EnsureSuccessStatus(msg);
            RespConsultarFechaRecepcionSii.Envelope res =
                XsltHelper.DeserializeStream<RespConsultarFechaRecepcionSii.Envelope>(
                    await msg.Content.ReadAsStreamAsync()
                );
            return res.Body!.consultarFechaRecepcionSiiResponse!.@return;
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

        public Task<RESPUESTA> ConsultaEstadoDteAv(
            string RutEmpresa,
            string DvEmpresa,
            string RutReceptor,
            string DvReceptor,
            string TipoDte,
            string FolioDte,
            string FechaEmisionDte,
            string MontoDte,
            string FirmaDte
        )
        {
            throw new NotImplementedException();
        }
    }
}

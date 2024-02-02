using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;

using Cve.Impuestos.Models;

using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

using ZXing;
using ZXing.PDF417;
using ZXing.PDF417.Internal;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;

namespace Cve.Impuestos.Helpers
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.Latin1;
    }

    public class XsltHelper
    {
        private readonly ILogger logger;
        private readonly XslCompiledTransform xlstFile = null!;
        private readonly XsltArgumentList xlstArgs = null!;
        public List<string> MensajeError { get; set; } = new List<string>();
        public List<string> MensajeWarning { get; set; } = new List<string>();
        private BarcodeWriter<Bitmap> barcodeWriter = null!;

        private void SetBarCode()
        {
            // https://www.sii.cl/factura_electronica/instructivo_emision.pdf Pag. 22
            // – Quiet Zone: Para evitar que líneas o textos cercanos al código puedan ser interpretados como
            // parte de éste, el código debe tener una Quiet Zone(espacio en blanco) de mínimo 0,25 pulgadas
            // alrededor de cada uno de sus cuatro lados.


            //– se debe utilizar el modo de codificación binario(Byte Compaction Mode)

            //– Error Correction Level(ECL): Dada la cantidad de información, se debe utilizar nivel 5.

            //– X Width(X Dim): Es el ancho del elemento impreso del código, barra o espacio, más angosto y
            //se expresa en mils(milésimas de pulgada). Se debe usar un valor de X Dim mínimo de 6,7 mils.

            //– Row Height(Y Dim): Es la dimensión vertical, expresada en mils, de una fila del código
            //PDF417.Se debe usar una relacion(3:1) respecto al valor X Dim.

            //– Recomendamos ajustar los parámetros para obtener un codigo de barras impreso de un tamaño
            //máximo de 3 cms de alto x 9 cms de ancho.



            barcodeWriter = new()
            {
                Format = BarcodeFormat.PDF_417,
                Options = new PDF417EncodingOptions
                {
                    ErrorCorrection = PDF417ErrorCorrectionLevel.L5,
                    Height = 4,
                    Width = 9,
                    Compaction = Compaction.BYTE,
                    Margin = 2
                },
                Renderer = new BitmapRenderer()
            };
        }

        public XsltHelper(ILogger logger)
        {
            this.logger = logger;
            xlstFile = new XslCompiledTransform(true);
            xlstArgs = new XsltArgumentList();
            using StringReader sr = new(Properties.Impuestos.Custodium);
            XmlReaderSettings settings = new() { Async = true };
            using XmlReader xr = XmlReader.Create(sr, settings);
            xlstFile.Load(xr);
            SetBarCode();
        }

        public XsltHelper()
        {
            logger = null!;
            xlstFile = new XslCompiledTransform();
            xlstArgs = new XsltArgumentList();
            using StringReader sr = new(Properties.Impuestos.Custodium);
            XmlReaderSettings settings = new() { Async = true };
            using XmlReader xr = XmlReader.Create(sr, settings);
            xlstFile.Load(xr);
            SetBarCode();
        }

        private string Transformar(string inputXml)
        {
            var ted = GetNode(inputXml);
            try
            {
                xlstArgs.Clear();
                using StringReader sr = new(inputXml);
                using XmlReader xr = XmlReader.Create(sr);
                using StringWriter sw = new();
                using MemoryStream ms = new();
                var timbtreTxt = ted;
                var timbre = barcodeWriter.Write(timbtreTxt);
                using Bitmap bitmap = new(timbre);
                bitmap.Save(ms, ImageFormat.Png);
                string SigBase64 = Convert.ToBase64String(ms.GetBuffer());
                xlstArgs.AddParam("TedTimbre", "", "data:image/png;base64," + SigBase64);
                xlstFile.Transform(xr, xlstArgs, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                // throw new Exception();
            }
            return null!;
        }

        private string GetNode(string inputXml)
        {
            XDocument t = XDocument.Parse(inputXml, LoadOptions.None);
            XElement? ted = t!
                .Descendants()
                .Elements()
                .ToList()
                .Where(c => c.Name.LocalName == "TED")
                .FirstOrDefault();
            // https://www.sii.cl/factura_electronica/instructivo_emision.pdf Pag.21
            Match match = Regex.Match(ted!.ToString(), @"(<DD.*?</DD>)", RegexOptions.Singleline);
            var ddelem = match.Value;
            if (match.Success)
            {
                ddelem = Regex.Replace(ddelem, @">\s+<", @"><");
            }
            return ddelem! == null ? null! : ddelem!;
        }

        // AQUI PODRÍA PONER LOS MÉTODOS DE SERIALIZACION (CAF!!!)
        public string SerializeObject<T>(T toSerialize)
        {
            // SERIALIZAR UN NODO DEL SET
            //XmlAttributeOverrides xmlOverrides = new();
            //xmlOverrides.Add(
            //    toSerialize!.GetType(),
            //    new XmlAttributes() { XmlRoot = new XmlRootAttribute("DTE") }
            //);

            XmlSerializer xmlSerializer = new(toSerialize!.GetType());
            using Utf8StringWriter textWriter = new();
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                MensajeWarning!.Add(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                MensajeError!.Add(e.Message);
                logger.LogError(e.Message);
            }
        }

        public async Task<T> LeerArchivos<T>(string path, CancellationToken canceltkn)
        {
            using FileStream stream = File.OpenRead(path);
            XDocument r = await XDocument.LoadAsync(stream, LoadOptions.None, canceltkn);
            XName first = r.Root!.Name;
            XmlReaderSettings settings = new() { ValidationType = ValidationType.Schema };
            switch (first.LocalName)
            {
                case "EnvioDTE":
                    settings.ValidationEventHandler += ValidationEventHandler!;
                    _ = settings.Schemas.Add(
                        "http://www.sii.cl/SiiDte",
                        @$"{Environment.CurrentDirectory}\Resources\EnvioDTE_v10.xsd"
                    );
                    _ = settings.Schemas.Add(
                        "http://www.sii.cl/SiiDte",
                        @$"{Environment.CurrentDirectory}\Resources\DTE_v10.xsd"
                    );
                    _ = settings.Schemas.Add(
                        "http://www.sii.cl/SiiDte",
                        @$"{Environment.CurrentDirectory}\Resources\SiiTypes_v10.xsd"
                    );
                    _ = settings.Schemas.Add(
                        "http://www.w3.org/2000/09/xmldsig#",
                        @$"{Environment.CurrentDirectory}\Resources\xmldsignature_v10.xsd"
                    );
                    EnvioDTE set = Deserializa<EnvioDTE>(settings, path);
                    return (T)Convert.ChangeType(set, typeof(T))!;
                case "RespuestaDTE":
                    break;
                case "EnvioRecibos":
                    break;
                case "DTE":
                    break;
                default:
                    break;
            }
            return (T)Convert.ChangeType(null, typeof(T))!;
        }

        private T Deserializa<T>(XmlReaderSettings settings, string path)
        {
            XmlSerializer ser = new(typeof(T));
            using XmlReader r = XmlReader.Create(path, settings);
            try
            {
                EnvioDTE? set = ser.Deserialize(r)! as EnvioDTE;
                return !MensajeError!.Any()
                    ? (T)Convert.ChangeType(set, typeof(T))!
                    : (T)Convert.ChangeType(null, typeof(T))!;
            }
            catch (Exception)
            {
                return (T)Convert.ChangeType(null, typeof(T))!;
            }
        }

        public async Task<string> GenerateHtml(string xml, string path = default!)
        {
            // PARA CONSULTAS INDIVIDUALES CON LA LUPA
            // DEBE RECIBIR UN NODO <DTE version="1.0" xmlns="http://www.sii.cl/SiiDte">
            // o <DTE version="1.0" charset="iso-8859-1">
            if (xml is null)
            {
                return null!;
            }

            string pathfile;
            try
            {
                pathfile = $"{Path.GetTempPath()}Html_{path}_{new Random().Next()}.html";
                var html = Transformar(xml);
                await File.WriteAllTextAsync(pathfile, html);
            }
            catch (IOException)
            {
                return null!;
            }
            return pathfile;
        }

        public string GenerateHtml(string xml)
        {
            // PARA CONSULTAS INDIVIDUALES CON LA LUPA
            // DEBE RECIBIR UN NODO <DTE version="1.0" xmlns="http://www.sii.cl/SiiDte">
            // o <DTE version="1.0" charset="iso-8859-1">
            if (xml is null)
            {
                return null!;
            }
            try
            {
                return Transformar(xml);
            }
            catch (IOException)
            {
                return null!;
            }
        }

        public static T DeserializeStream<T>(Stream xml)
        {
            // QUITAR STATIC SI SE REQUIERE E INSTANCIAR EN CADA CLASE QEU LA PIDE
            XmlSerializer serializer = new(typeof(T));
            using StreamReader reader = new(xml);
            return (T)serializer.Deserialize(reader)!;
        }

        public T Serialize<T>(object req)
        {
            XmlSerializer x = new(req.GetType());
            using Utf8StringWriter txt = new();
            x.Serialize(
                XmlWriter.Create(txt, new XmlWriterSettings() { Indent = false, Async = true }),
                req,
                new XmlSerializerNamespaces(
                    new[] { new XmlQualifiedName(string.Empty, string.Empty) }
                )
            );

            return default!;
        }

        public T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new(typeof(T));
            using StringReader reader = new(xml);
            return (T)serializer.Deserialize(reader)!;
        }
    }
}


using PortalFacturas.Properties;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

using ZXing;
using ZXing.PDF417;
using ZXing.PDF417.Internal;

namespace PortalFacturas.Services
{
    public static class XmlTransformHelperExtensions
    {
        public static async Task<IXslMapperFunctionService> AddParam(this Task<IXslMapperFunctionService> helper, byte[] inputXml)
        {
            IXslMapperFunctionService instance = await helper;
            return await instance.AddParam(inputXml);
        }

        public static async Task<byte[]> TransformAsync(this Task<IXslMapperFunctionService> helper, byte[] inputXml)
        {
            IXslMapperFunctionService instance = await helper;
            return await instance.TransformAsync(inputXml);
        }
    }
    public interface IXslMapperFunctionService
    {
        Task<byte[]> TransformAsync(byte[] inputXml);
        Task<IXslMapperFunctionService> AddParam(byte[] inputXml);
        Task<IXslMapperFunctionService> LoadXslAsync();
    }

    public class XslMapperFunctionService : IXslMapperFunctionService
    {
        private readonly HttpClient _httpClient;
        private readonly XslCompiledTransform _xslt;
        private readonly XsltArgumentList _arguments;


        public XslMapperFunctionService(HttpClient _httpClient)
        {
            this._httpClient = _httpClient;
            _xslt = new XslCompiledTransform(enableDebug: false);
            _arguments = new XsltArgumentList();
        }

        public async Task<IXslMapperFunctionService> AddParam(byte[] inputXml)
        {
            XmlDocument doc = new();
            XmlNode ted = LoadXmlDocument(doc, Encoding.UTF8.GetString(inputXml));
            try
            {
                await Task.Run(() =>
                {
                    BarcodeWriter barcodeWriter = new BarcodeWriter()
                    {
                        Format = BarcodeFormat.PDF_417,
                        Options = new PDF417EncodingOptions
                        {
                            ErrorCorrection = PDF417ErrorCorrectionLevel.L6,
                            Height = 3,
                            Width = 9,
                            Compaction = Compaction.BYTE,
                            Margin = 6
                        }
                    };
                    Bitmap timbre = barcodeWriter.Write(ted.OuterXml);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        StringReader _transformStringReader = new(Encoding.UTF8.GetString(inputXml));
                        XmlReader _transformTextReader = XmlReader.Create(_transformStringReader);
                        XmlWriter _transformWriter = XmlWriter.Create(ms, _xslt.OutputSettings);

                        using (Bitmap bitmap = new Bitmap(timbre))
                        {
                            bitmap.Save(ms, ImageFormat.Png);
                            string SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                            _arguments.AddParam("TedTimbre", "", "data:image/png;base64," + SigBase64);
                            _xslt.Transform(_transformTextReader, _arguments, _transformWriter);
                        }
                        return this;
                    }

                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Timbre!!!!{ex.Message}");
            }
            return this;

        }

        public async Task<byte[]> TransformAsync(byte[] inputXml)
        {
            try
            {
                using (MemoryStream _transformStream = new MemoryStream())
                {
                    await Task.Run(() =>
                    {
                        StringReader _transformStringReader = new(Encoding.UTF8.GetString(inputXml));
                        XmlReader _transformTextReader = XmlReader.Create(_transformStringReader);
                        XmlWriter _transformWriter = XmlWriter.Create(_transformStream, _xslt.OutputSettings);
                        _xslt.Transform(_transformTextReader, _arguments, _transformWriter);
                    });
                    return _transformStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"TransformAsync{ ex.Message }");
            }

        }

        private XmlNode LoadXmlDocument(XmlDocument xml, string doc)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml) + "eeerrrrrrr");
            }
            if (string.IsNullOrWhiteSpace(doc))
            {
                throw new ArgumentNullException(nameof(doc) + "rrrrrrrrrrr");
            }
            try
            {
                xml.LoadXml(doc);
                XmlNodeList nodeTED = xml.GetElementsByTagName("TED");
                if (nodeTED.Count != 1)
                {
                    throw new Exception("LoadXmlDocument");
                }
                return nodeTED[0];

            }
            catch (Exception ex)
            {

                throw new Exception($"LoadXmlDocument {ex.Message}");
            }
        }

        public async Task<IXslMapperFunctionService> LoadXslAsync()
        {
            using (StringReader reader = new(Resources.Custodium))
            {
                try
                {
                    XmlReader xsltReader = XmlReader.Create(reader);
                    await Task.Factory.StartNew(() => _xslt
                    .Load(xsltReader, new XsltSettings(
                        enableDocumentFunction: true,
                        enableScript: true), stylesheetResolver: null));

                }
                catch (Exception ex)
                {
                    throw new Exception($"ERROR !!!!! XSLT {ex.Message}");
                }
            }
            return this;
        }
    }
}

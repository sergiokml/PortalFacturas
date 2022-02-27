using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

using PortalFacturas.Interfaces;
using PortalFacturas.Properties;

using ZXing;
using ZXing.PDF417;
using ZXing.PDF417.Internal;

namespace PortalFacturas.Helpers
{
    public class XsltHelper : IXsltHelper
    {
        //private readonly HttpClient _httpClient;
        private readonly XslCompiledTransform _xslt;
        private readonly XsltArgumentList _arguments;

        public XsltHelper()
        {
            // this._httpClient = _httpClient;
            _xslt = new XslCompiledTransform(enableDebug: false);
            _arguments = new XsltArgumentList();
        }

        public async Task<IXsltHelper> AddParam(byte[] inputXml)
        {
            XmlDocument doc = new();
            XmlNode ted = LoadXmlDocument(doc, Encoding.UTF8.GetString(inputXml));
            try
            {
                await Task.Run(
                    () =>
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
                            StringReader _transformStringReader =
                                new(Encoding.UTF8.GetString(inputXml));
                            XmlReader _transformTextReader = XmlReader.Create(
                                _transformStringReader
                            );
                            XmlWriter _transformWriter = XmlWriter.Create(ms, _xslt.OutputSettings);

                            using (Bitmap bitmap = new Bitmap(timbre))
                            {
                                bitmap.Save(ms, ImageFormat.Png);
                                string SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64
                                _arguments.AddParam(
                                    "TedTimbre",
                                    "",
                                    "data:image/png;base64," + SigBase64
                                );
                                _xslt.Transform(_transformTextReader, _arguments, _transformWriter);
                            }
                            return this;
                        }
                    }
                );
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return this;
        }

        public async Task<byte[]> TransformAsync(byte[] inputXml)
        {
            try
            {
                using (MemoryStream _transformStream = new MemoryStream())
                {
                    await Task.Run(
                        () =>
                        {
                            StringReader _transformStringReader =
                                new(Encoding.UTF8.GetString(inputXml));
                            XmlReader _transformTextReader = XmlReader.Create(
                                _transformStringReader
                            );
                            XmlWriter _transformWriter = XmlWriter.Create(
                                _transformStream,
                                _xslt.OutputSettings
                            );
                            _xslt.Transform(_transformTextReader, _arguments, _transformWriter);
                        }
                    );
                    return _transformStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private XmlNode LoadXmlDocument(XmlDocument xml, string doc)
        {
            xml.LoadXml(doc);
            XmlNodeList nodeTED = xml.GetElementsByTagName("TED");
            if (nodeTED.Count != 1)
            {
                throw new Exception("LoadXmlDocument");
            }
            return nodeTED[0];
        }

        public async Task<IXsltHelper> LoadXslAsync()
        {
            using (StringReader reader = new(Resources.Custodium))
            {
                try
                {
                    XmlReader xsltReader = XmlReader.Create(reader);
                    await Task.Factory.StartNew(
                        () =>
                            _xslt.Load(
                                xsltReader,
                                new XsltSettings(enableDocumentFunction: true, enableScript: true),
                                stylesheetResolver: null
                            )
                    );
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            return this;
        }
    }
}

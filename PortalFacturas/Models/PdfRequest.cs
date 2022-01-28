using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class PdfRequest
    {
        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        //[JsonPropertyName("options")]
        //public Options Options { get; set; }

        [JsonPropertyName("useCustomStorage")]
        public bool UseCustomStorage { get; set; }

        //[JsonPropertyName("storage")]
        //public Storage Storage { get; set; }
    }

    public class Options
    {
        [JsonPropertyName("delay")]
        public int Delay { get; set; }

        [JsonPropertyName("puppeteerWaitForMethod")]
        public string PuppeteerWaitForMethod { get; set; }

        [JsonPropertyName("puppeteerWaitForValue")]
        public string PuppeteerWaitForValue { get; set; }

        [JsonPropertyName("usePrintCss")]
        public bool UsePrintCss { get; set; }

        [JsonPropertyName("landscape")]
        public bool Landscape { get; set; }

        [JsonPropertyName("printBackground")]
        public bool PrintBackground { get; set; }

        [JsonPropertyName("displayHeaderFooter")]
        public bool DisplayHeaderFooter { get; set; }

        [JsonPropertyName("headerTemplate")]
        public string HeaderTemplate { get; set; }

        [JsonPropertyName("footerTemplate")]
        public string FooterTemplate { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("marginTop")]
        public string MarginTop { get; set; }

        [JsonPropertyName("marginBottom")]
        public string MarginBottom { get; set; }

        [JsonPropertyName("marginLeft")]
        public string MarginLeft { get; set; }

        [JsonPropertyName("marginRight")]
        public string MarginRight { get; set; }

        [JsonPropertyName("pageRanges")]
        public string PageRanges { get; set; }

        [JsonPropertyName("scale")]
        public int Scale { get; set; }

        [JsonPropertyName("omitBackground")]
        public bool OmitBackground { get; set; }
    }

    public class ExtraHTTPHeaders { }

    public class Storage
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("extraHTTPHeaders")]
        public ExtraHTTPHeaders ExtraHTTPHeaders { get; set; }
    }
}

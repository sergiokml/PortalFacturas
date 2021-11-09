using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class ResponsePdfRestPackModel
    {
        [JsonPropertyName("cached")]
        public string Cached { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("remote_status")]
        public string RemoteStatus { get; set; }

        [JsonPropertyName("run_time")]
        public string RunTime { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public string Width { get; set; }
    }
}

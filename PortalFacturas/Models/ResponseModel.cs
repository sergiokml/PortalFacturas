using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("$content-type")]
        public string ContentType { get; set; }

        [JsonPropertyName("$content")]
        public string Content { get; set; }
    }
}

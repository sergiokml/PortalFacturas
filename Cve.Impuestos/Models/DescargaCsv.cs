using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    public class DescargaCsv
    {
        [JsonPropertyName("data")]
        public string[]? Data { get; set; }

        [JsonPropertyName("metaData")]
        public MDResumenResp? MetaData { get; set; }

        [JsonPropertyName("respEstado")]
        public RespEstado? RespEstado { get; set; }

        [JsonPropertyName("nombreArchivo")]
        public string? NombreArchivo { get; set; }
    }
}

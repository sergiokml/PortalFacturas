using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    public class CaptchaModel
    {
        [JsonPropertyName("codigorespuesta")]
        public int Codigorespuesta { get; set; }

        [JsonPropertyName("glosarespuesta")]
        public string? Glosarespuesta { get; set; }

        [JsonPropertyName("imgCaptcha")]
        public object? ImgCaptcha { get; set; }

        [JsonPropertyName("txtCaptcha")]
        public string? TxtCaptcha { get; set; }

        [JsonPropertyName("largoCaptcha")]
        public object? LargoCaptcha { get; set; }

        [JsonPropertyName("validez")]
        public bool Validez { get; set; }
    }
}

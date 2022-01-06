using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class TokenSp
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonPropertyName("ext_expires_in")]
        public string ExtExpiresIn { get; set; }

        [JsonPropertyName("expires_on")]
        public string ExpiresOn { get; set; }

        [JsonPropertyName("not_before")]
        public string NotBefore { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }

    public class TokenCen
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}

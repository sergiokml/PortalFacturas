using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    public class ResumenReq
    {
        public ResumenReq(MDResumen metaData, DResumen data)
        {
            MetaData = metaData;
            Data = data;
        }

        [JsonPropertyName("metaData")]
        public MDResumen MetaData { get; set; }

        [JsonPropertyName("data")]
        public DResumen Data { get; set; }
    }

    public class MDResumen
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("conversationId")]
        public string? ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; set; }

        [JsonPropertyName("page")]
        public object? Page { get; set; }
    }

    public class DResumen
    {
        [JsonPropertyName("periodo")]
        public string? Periodo { get; set; }

        [JsonPropertyName("rutContribuyente")]
        public string? RutContribuyente { get; set; }

        [JsonPropertyName("dvContribuyente")]
        public string? DvContribuyente { get; set; }

        [JsonPropertyName("operacion")]
        public int? Operacion { get; set; }
    }
}

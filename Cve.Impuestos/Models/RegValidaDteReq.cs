using System.Text.Json.Serialization;

namespace Cve.Impuestos.Models
{
    public class RegValidaDteReq
    {
        [JsonPropertyName("metaData")]
        public MetaDataRegValidaDteReqModel? MetaData { get; set; }

        [JsonPropertyName("data")]
        public DataRegValidaDteReqModel? Data { get; set; }

        public RegValidaDteReq(
            MetaDataRegValidaDteReqModel? metaData,
            DataRegValidaDteReqModel? data
        )
        {
            MetaData = metaData;
            Data = data;
        }
    }

    public class DataRegValidaDteReqModel
    {
        [JsonPropertyName("rutEmisor")]
        public string? RutEmisor { get; set; }

        [JsonPropertyName("dvEmisor")]
        public string? DvEmisor { get; set; }

        [JsonPropertyName("tipoDoc")]
        public string? TipoDoc { get; set; }

        [JsonPropertyName("folio")]
        public string? Folio { get; set; }

        [JsonPropertyName("rutToken")]
        public string? RutToken { get; set; }

        [JsonPropertyName("dvToken")]
        public string? DvToken { get; set; }
    }

    public class MetaDataRegValidaDteReqModel
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
}

using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class BaseModel<T>
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public object? Next { get; set; }

        [JsonPropertyName("previous")]
        public object? Previous { get; set; }

        [JsonPropertyName("results")]
        public List<T>? Results { get; set; }

        [JsonPropertyName("result")]
        public T? Result { get; set; } // Response POST DTE

        [JsonPropertyName("errors")]
        public List<string>? errors { get; set; }

        [JsonPropertyName("operation")]
        public int operation { get; set; }
    }

    public class DeletedObjectsDetail
    {
        [JsonPropertyName("payments.DTE")]
        public int PaymentsDTE { get; set; }
    }

    public class DteDeleteResult
    {
        [JsonPropertyName("deleted_objects_count")]
        public int DeletedObjectsCount { get; set; }

        [JsonPropertyName("deleted_objects_detail")]
        public DeletedObjectsDetail? DeletedObjectsDetail { get; set; }

        [JsonPropertyName("deleted_dte_id")]
        public string? DeletedDteId { get; set; }
    }

    public class DteDeleteModel
    {
        [JsonPropertyName("result")]
        public DteDeleteResult? Result { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("operation")]
        public int Operation { get; set; }
    }
}

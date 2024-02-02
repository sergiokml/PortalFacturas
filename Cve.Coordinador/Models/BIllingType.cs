using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class BillingType
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("natural_key")]
        public string? NaturalKey { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("system_prefix")]
        public string? SystemPrefix { get; set; }

        [JsonPropertyName("description_prefix")]
        public string? DescriptionPrefix { get; set; }

        [JsonPropertyName("payment_window")]
        public int PaymentWindow { get; set; }

        [JsonPropertyName("department")]
        public int Department { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    //public class BIllingType
    //{
    //    [JsonPropertyName("count")]
    //    public int Count { get; set; }

    //    [JsonPropertyName("next")]
    //    public object? Next { get; set; }

    //    [JsonPropertyName("previous")]
    //    public object? Previous { get; set; }

    //    [JsonPropertyName("results")]
    //    public List<BillingTypeResult>? Results { get; set; }
    //}
}

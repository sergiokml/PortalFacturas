using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class BillingWindow
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("natural_key")]
        public string? NaturalKey { get; set; }

        [JsonPropertyName("billing_type")]
        public int BillingType { get; set; }

        [JsonPropertyName("periods")]
        public List<string>? Periods { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }

    //public class BillingWindow
    //{
    //    [JsonPropertyName("count")]
    //    public int Count { get; set; }

    //    [JsonPropertyName("next")]
    //    public object? Next { get; set; }

    //    [JsonPropertyName("previous")]
    //    public object? Previous { get; set; }

    //    [JsonPropertyName("results")]
    //    public List<BillingWindowResult>? Results { get; set; }
    //}
}

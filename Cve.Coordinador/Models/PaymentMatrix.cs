using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class PaymentMatrix
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("auxiliary_data")]
        public AuxiliaryData? AuxiliaryData { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("payment_file")]
        public string? PaymentFile { get; set; }

        [JsonPropertyName("letter_code")]
        public string? LetterCode { get; set; }

        [JsonPropertyName("letter_year")]
        public int LetterYear { get; set; }

        [JsonPropertyName("letter_file")]
        public string? LetterFile { get; set; }

        [JsonPropertyName("matrix_file")]
        public string? MatrixFile { get; set; }

        [JsonPropertyName("publish_date")]
        public string? PublishDate { get; set; }

        [JsonPropertyName("payment_days")]
        public int? PaymentDays { get; set; }

        [JsonPropertyName("payment_date")]
        public string? PaymentDate { get; set; }

        [JsonPropertyName("billing_date")]
        public string? BillingDate { get; set; }

        [JsonPropertyName("payment_window")]
        public int PaymentWindow { get; set; }

        [JsonPropertyName("natural_key")]
        public string? NaturalKey { get; set; }

        [JsonPropertyName("reference_code")]
        public string? ReferenceCode { get; set; }

        [JsonPropertyName("billing_window")]
        public int BillingWindow { get; set; }

        [JsonPropertyName("payment_due_type")]
        public int PaymentDueType { get; set; }
    }
}

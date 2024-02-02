using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class CreditorJobResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("auxiliary_data")]
        public AuxiliaryData? AuxiliaryData { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }

        [JsonPropertyName("items_count")]
        public int? ItemsCount { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("insert_count")]
        public int? InsertCount { get; set; }

        [JsonPropertyName("update_count")]
        public int? UpdateCount { get; set; }

        [JsonPropertyName("delete_count")]
        public int? DeleteCount { get; set; }

        [JsonPropertyName("unchanged_count")]
        public int? UnchangedCount { get; set; }

        [JsonPropertyName("current_step")]
        public string? CurrentStep { get; set; }

        [JsonPropertyName("successful")]
        public bool Successful { get; set; }

        [JsonPropertyName("expired")]
        public bool Expired { get; set; }

        [JsonPropertyName("started_at")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("finished_at")]
        public DateTime FinishedAt { get; set; }

        [JsonPropertyName("user")]
        public int User { get; set; }

        [JsonPropertyName("auxiliary_file")]
        public int AuxiliaryFile { get; set; }

        [JsonPropertyName("participant")]
        public int Participant { get; set; }

        [JsonPropertyName("payment_matrix")]
        public int PaymentMatrix { get; set; }
    }

    public class CreditorJob
    {
        [JsonPropertyName("result")]
        public CreditorJobResult? Result { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("operation")]
        public int Operation { get; set; }
    }
}

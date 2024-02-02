using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class Dte
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("relative_emission_file")]
        public string? RelativeEmissionFile { get; set; }

        [JsonPropertyName("instruction")]
        public int Instruction { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("type_sii_code")]
        public int TypeSiiCode { get; set; } // FOR POST.

        [JsonPropertyName("folio")]
        public int Folio { get; set; }

        [JsonPropertyName("gross_amount")]
        public long GrossAmount { get; set; }

        [JsonPropertyName("net_amount")]
        public long NetAmount { get; set; }

        [JsonPropertyName("reported_by_creditor")]
        public bool ReportedByCreditor { get; set; }

        [JsonPropertyName("emission_dt")]
        public string? EmissionDt { get; set; }

        [JsonPropertyName("emission_file")]
        public string? EmissionFile { get; set; }

        [JsonPropertyName("emission_erp_a")]
        public string? EmissionErpA { get; set; }

        [JsonPropertyName("emission_erp_b")]
        public string? EmissionErpB { get; set; }

        [JsonPropertyName("reception_dt")]
        public string? ReceptionDt { get; set; }

        [JsonPropertyName("reception_erp")]
        public string? ReceptionErp { get; set; }

        [JsonPropertyName("acceptance_dt")]
        public object? AcceptanceDt { get; set; }

        [JsonPropertyName("acceptance_erp")]
        public object? AcceptanceErp { get; set; }

        [JsonPropertyName("acceptance_status")]
        public object? AcceptanceStatus { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public string? UpdatedTs { get; set; }
    }

    //public class Dte
    //{
    //    [JsonPropertyName("count")]
    //    public int Count { get; set; }

    //    [JsonPropertyName("next")]
    //    public object? Next { get; set; }

    //    [JsonPropertyName("previous")]
    //    public object? Previous { get; set; }

    //    [JsonPropertyName("results")]
    //    public List<DteResult>? Results { get; set; }

    //    [JsonPropertyName("result")]
    //    public DteResult? Result { get; set; } // Response POST DTE

    //    [JsonPropertyName("errors")]
    //    public List<string>? errors { get; set; }

    //    [JsonPropertyName("operation")]
    //    public int operation { get; set; }
    //}


}

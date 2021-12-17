using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class DteResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("relative_emission_file")]
        public string RelativeEmissionFile { get; set; }

        [JsonPropertyName("instruction")]
        public int Instruction { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("folio")]
        public int Folio { get; set; }

        [JsonPropertyName("gross_amount")]
        public int GrossAmount { get; set; }

        [JsonPropertyName("net_amount")]
        public int NetAmount { get; set; }

        [JsonPropertyName("reported_by_creditor")]
        public bool ReportedByCreditor { get; set; }

        [JsonPropertyName("emission_dt")]
        public DateTime? EmissionDt { get; set; }

        [JsonPropertyName("emission_file")]
        public string EmissionFile { get; set; }

        [JsonPropertyName("emission_erp_a")]
        public string EmissionErpA { get; set; }

        [JsonPropertyName("emission_erp_b")]
        public object EmissionErpB { get; set; }

        [JsonPropertyName("reception_dt")]
        public string ReceptionDt { get; set; }

        [JsonPropertyName("reception_erp")]
        public object ReceptionErp { get; set; }

        [JsonPropertyName("acceptance_dt")]
        public string AcceptanceDt { get; set; }

        [JsonPropertyName("acceptance_erp")]
        public object AcceptanceErp { get; set; }

        [JsonPropertyName("acceptance_status")]
        public int? AcceptanceStatus { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }

    public class DteModel
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public object Next { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<DteResult> Results { get; set; }
    }
}

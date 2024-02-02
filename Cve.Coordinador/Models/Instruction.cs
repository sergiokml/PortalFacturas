using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models;

public class AuxiliaryData
{
    [JsonPropertyName("payment_matrix_natural_key")]
    public string? PaymentMatrixNaturalKey { get; set; }

    [JsonPropertyName("payment_matrix_concept")]
    public string? PaymentMatrixConcept { get; set; }

    [JsonPropertyName("payment_matrix_publication")]
    public DateTime PaymentMatrixPublication { get; set; }

    [JsonPropertyName("payment_matrix_payment_date")]
    public string? PaymentMatrixPaymentDate { get; set; }

    [JsonPropertyName("creditor_name")]
    public string? CreditorName { get; set; }

    [JsonPropertyName("debtor_name")]
    public string? DebtorName { get; set; }
}

public class Instruction
{
    //public string? Empresa { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("payment_matrix")]
    public int PaymentMatrix { get; set; }

    [JsonPropertyName("creditor")]
    public int Creditor { get; set; }

    [JsonPropertyName("debtor")]
    public int Debtor { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("closed")]
    public bool Closed { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("resolution")]
    public string? Resolution { get; set; }

    [JsonPropertyName("max_payment_date")]
    public DateTime? MaxPaymentDate { get; set; }

    [JsonPropertyName("informed_paid_amount")]
    public int InformedPaidAmount { get; set; }

    [JsonPropertyName("is_paid")]
    public bool IsPaid { get; set; }

    [JsonPropertyName("auxiliary_data")]
    public AuxiliaryData? AuxiliaryData { get; set; }

    [JsonPropertyName("created_ts")]
    public DateTime CreatedTs { get; set; }

    [JsonPropertyName("updated_ts")]
    public DateTime UpdatedTs { get; set; }

    //GUARDO DTE`S PARA PORTAL FACTURAS
    public List<Dte>? DteAsociados { get; set; } = new List<Dte>();
}

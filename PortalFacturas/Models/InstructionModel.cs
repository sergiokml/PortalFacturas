// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace PortalFacturas.Models
{
    public class AuxiliaryData
    {
        [JsonPropertyName("payment_matrix_natural_key")]
        public string PaymentMatrixNaturalKey { get; set; }

        [JsonPropertyName("payment_matrix_concept")]
        public string PaymentMatrixConcept { get; set; }

        [JsonPropertyName("payment_matrix_publication")]
        public DateTime PaymentMatrixPublication { get; set; }

        [JsonPropertyName("payment_matrix_payment_date")]
        public object PaymentMatrixPaymentDate { get; set; }

        [JsonPropertyName("creditor_name")]
        public string CreditorName { get; set; }

        [JsonPropertyName("debtor_name")]
        public string DebtorName { get; set; }
    }

    public class InstructionResult
    {
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

        [JsonPropertyName("amount_gross")]
        public int AmountGross { get; set; }

        [JsonPropertyName("closed")]
        public bool Closed { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("status_billed")]
        public int StatusBilled { get; set; }

        [JsonPropertyName("status_paid")]
        public int StatusPaid { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("max_payment_date")]
        public string MaxPaymentDate { get; set; }

        [JsonPropertyName("informed_paid_amount")]
        public int InformedPaidAmount { get; set; }

        [JsonPropertyName("is_paid")]
        public bool IsPaid { get; set; }

        [JsonPropertyName("auxiliary_data")]
        public AuxiliaryData AuxiliaryData { get; set; }

        [JsonPropertyName("accept_partial_payments")]
        public bool AcceptPartialPayments { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }

        public DteResult DteResult { get; set; }
    }

    public class InstructionModel
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public object Next { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<InstructionResult> Results { get; set; }
    }
}


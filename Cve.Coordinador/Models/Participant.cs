using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class PaymentsContact
    {
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string>? Phones { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class BillsContact
    {
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string>? Phones { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class Participant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("is_coordinator")]
        public bool IsCoordinator { get; set; }

        [JsonPropertyName("participant")]
        public int ParticipantID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rut")]
        public int Rut { get; set; }

        [JsonPropertyName("verification_code")]
        public string? VerificationCode { get; set; }

        [JsonPropertyName("business_name")]
        public string? BusinessName { get; set; }

        [JsonPropertyName("commercial_business")]
        public string? CommercialBusiness { get; set; }

        [JsonPropertyName("dte_reception_email")]
        public string? DteReceptionEmail { get; set; }

        [JsonPropertyName("bank_account")]
        public string? BankAccount { get; set; }

        [JsonPropertyName("bank")]
        public int? Bank { get; set; }

        [JsonPropertyName("commercial_address")]
        public string? CommercialAddress { get; set; }

        [JsonPropertyName("postal_address")]
        public string? PostalAddress { get; set; }

        [JsonPropertyName("manager")]
        public string? Manager { get; set; }

        [JsonPropertyName("payments_contact")]
        public PaymentsContact? PaymentsContact { get; set; }

        [JsonPropertyName("bills_contact")]
        public BillsContact? BillsContact { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }
}

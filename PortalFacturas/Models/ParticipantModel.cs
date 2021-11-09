using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class PaymentsContact
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string> Phones { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class BillsContact
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string> Phones { get; set; }

        [JsonPropertyName("email")]

        public string Email { get; set; }
    }

    public class ParticipantResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rut")]
        public int Rut { get; set; }

        [JsonPropertyName("verification_code")]
        public string VerificationCode { get; set; }

        [JsonPropertyName("business_name")]
        public string BusinessName { get; set; }

        [JsonPropertyName("commercial_business")]
        public string CommercialBusiness { get; set; }

        [JsonPropertyName("dte_reception_email")]
        public string DteReceptionEmail { get; set; }

        [JsonPropertyName("bank_account")]
        public string BankAccount { get; set; }

        [JsonIgnore]
        public int Bank { get; set; }

        [JsonPropertyName("commercial_address")]
        public string CommercialAddress { get; set; }

        [JsonPropertyName("postal_address")]
        public string PostalAddress { get; set; }

        [JsonPropertyName("manager")]
        public string Manager { get; set; }

        [JsonPropertyName("payments_contact")]
        public PaymentsContact PaymentsContact { get; set; }

        [JsonPropertyName("bills_contact")]
        public BillsContact BillsContact { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }

    public class ParticipantModel
    {
        [JsonPropertyName("count"), JsonNumberHandling(JsonNumberHandling.WriteAsString)]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<ParticipantResult> Results { get; set; }
    }
}


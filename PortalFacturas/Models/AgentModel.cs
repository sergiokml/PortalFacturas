using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortalFacturas.Models
{
    public class Participant
    {
        [JsonPropertyName("is_coordinator")]
        public bool IsCoordinator { get; set; }

        [JsonPropertyName("participant")]
        public int ParticipantID { get; set; }
    }

    public class AgentResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string> Phones { get; set; }

        [JsonPropertyName("profile")]
        public int Profile { get; set; }

        [JsonPropertyName("participants")]
        public List<Participant> Participants { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }

    public class AgentModel
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public object Next { get; set; }

        [JsonPropertyName("previous")]
        public object Previous { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<AgentResult> Results { get; set; }
    }
}


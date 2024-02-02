using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class Agent
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phones")]
        public List<string>? Phones { get; set; }

        [JsonPropertyName("profile")]
        public int Profile { get; set; }

        [JsonPropertyName("participants")]
        public List<Participant>? Participants { get; set; }

        [JsonPropertyName("created_ts")]
        public DateTime CreatedTs { get; set; }

        [JsonPropertyName("updated_ts")]
        public DateTime UpdatedTs { get; set; }
    }

    //public class Agent
    //{
    //    [JsonPropertyName("count")]
    //    public int Count { get; set; }

    //    [JsonPropertyName("next")]
    //    public object? Next { get; set; }

    //    [JsonPropertyName("previous")]
    //    public object? Previous { get; set; }

    //    [JsonPropertyName("results")]
    //    public List<AgentResult>? Results { get; set; }
    //}
}

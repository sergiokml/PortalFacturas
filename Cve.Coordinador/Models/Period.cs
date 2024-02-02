using System.Text.Json.Serialization;

namespace Cve.Coordinador.Models
{
    public class Period
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}

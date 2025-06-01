using System.Text.Json.Serialization;

namespace satelliteBackend.Models.Dto
{
    public class AnalysisDetail
    {
        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace satelliteBackend.Models.Dto
{
    public class AnalysisDto
    {
        public string Date { get; set; }

        [JsonPropertyName("ortalama")]
        public double Average { get; set; }

        [JsonPropertyName("yorum")]
        public string Comment { get; set; }

        [JsonPropertyName("tavsiye")]
        public string Recommendation { get; set; }

        [JsonPropertyName("detayli_analiz")]
        public Dictionary<string, AnalysisDetail> Details { get; set; }
    }
}

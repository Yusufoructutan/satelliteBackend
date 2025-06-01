using System.Text.Json.Serialization;

namespace satelliteBackend.Models.Dto
{
    public class PythonResponseDto
    {
        public List<string> Images { get; set; } // Görüntüler, base64 olarak liste halinde
        public int LocationId { get; set; } // Foreign key
        [JsonPropertyName("analyses")]
        public List<AnalysisDto> Analyses { get; set; }


    }
}

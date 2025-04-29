namespace satelliteBackend.Models.Dto
{
    public class PythonResponseDto
    {
        public List<string> Images { get; set; } // Görüntüler, base64 olarak liste halinde
        public int LocationId { get; set; } // Foreign key
    }
}

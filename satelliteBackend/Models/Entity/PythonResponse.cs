public class PythonResponse
{
    public int Id { get; set; } // İd alanı ekleyin
    public List<string>? Images { get; set; }  // Birden fazla resmi tutmak için liste
    public int LocationId { get; set; } // Foreign key
    public Location Location { get; set; } // Navigation property
}

using System.ComponentModel.DataAnnotations.Schema;

public class LocationImage
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string ImageBase64 { get; set; }

    public string ImageType { get; set; } // örnek: "RGB" ya da "NDVI"

    public DateTime ImageDate { get; set; }

    public int LocationId { get; set; }
    public Location Location { get; set; }  // Navigation property
}

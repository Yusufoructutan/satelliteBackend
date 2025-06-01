using System.ComponentModel.DataAnnotations.Schema;

public class LocationImage
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string ImageBase64 { get; set; }

    public int LocationId { get; set; }
    public Location Location { get; set; }  // Navigation property
}

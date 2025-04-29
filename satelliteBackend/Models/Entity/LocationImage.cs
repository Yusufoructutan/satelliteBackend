public class LocationImage
{
    public int Id { get; set; }
    public string ImageBase64 { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }  // Navigation property
}

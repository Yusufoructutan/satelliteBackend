public class Location
{
    public int Id { get; set; }
    public double SouthWestLatitude { get; set; }
    public double SouthWestLongitude { get; set; }
    public double NorthEastLatitude { get; set; }
    public double NorthEastLongitude { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public ICollection<LocationImage> Images { get; set; } = new List<LocationImage>();

    public ICollection<WeatherForecast> WeatherForecasts { get; set; } // burası!


}

public class LocationWithWeatherDto
{
    public int Id { get; set; }
    public double SouthWestLatitude { get; set; }
    public double SouthWestLongitude { get; set; }
    public double NorthEastLatitude { get; set; }
    public double NorthEastLongitude { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Hava durumu bilgileri
    public DateTime ForecastDate { get; set; }
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }
    public string WeatherDescription { get; set; }
}

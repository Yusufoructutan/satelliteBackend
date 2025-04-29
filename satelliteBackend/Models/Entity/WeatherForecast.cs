public class WeatherForecast
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public DateTime ForecastDate { get; set; }
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }
    public string WeatherDescription { get; set; }

    // Yeni Eklenen Özellikler
    public double? Humidity { get; set; }  // Nem
    public double? Precipitation { get; set; }  // Yağış
    public double? UvIndex { get; set; }  // UV Endeksi

    public Location Location { get; set; }
}

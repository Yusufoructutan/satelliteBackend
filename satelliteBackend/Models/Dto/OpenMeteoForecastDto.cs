public class OpenMeteoForecastDto
{
    public DateTime Time { get; set; }
    public double TemperatureC { get; set; }
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }

    public double Precipitation { get; set; }  // Yağış
    public double HumidityMax { get; set; }  // Nem
    public double UvIndexMax { get; set; }  // UV Endeksi
}

namespace satelliteBackend.Models.Dto
{
    public class WeatherForecastDto
    {
        public DateTime ForecastDate { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double? Humidity { get; set; }
        public double? Precipitation { get; set; }
        public double? UvIndex { get; set; }
        public string WeatherDescription { get; set; }
    }
}

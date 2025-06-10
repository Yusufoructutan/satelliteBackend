namespace satelliteBackend.Models.Dto
{
    public class LocationDetailsResponseDto
    {
        public int LocationId { get; set; }
        public List<WeatherForecastDto> WeatherForecasts { get; set; }
        public List<AnalysisDto> Analyses { get; set; }
    }
}

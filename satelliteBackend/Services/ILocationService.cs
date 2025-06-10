using satelliteBackend.Models.Dto;

public interface ILocationService
{
    Task<object> GetWeatherAndImagesByLocationId(int locationId);
    Task<List<WeatherForecastDto>> GetWeatherDataByLocationId(int locationId);
    Task<List<ImageDto>> GetImagesByLocationId(int locationId);
    Task<List<LocationImage>> SendToPythonService(Location location);
    Task<List<OpenMeteoForecastDto>> GetWeatherForecastForLocationAsync(Location location);
    Task<object> AddLocationAsync(LocationDto locationDto, int userId);
    Task<List<ImageDto>> GetUserImagesAsync(int userId);
    Task<LocationDto> GetLocationByIdAsync(int locationId);
    Task<object> DeleteLocationAsync(int locationId, int userId);

    Task<LocationDetailsResponseDto?> GetWeatherAndAnalysesByLocationId(int locationId);

}

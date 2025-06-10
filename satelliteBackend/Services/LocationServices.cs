using Microsoft.EntityFrameworkCore;
using MyAspNetCoreProject.Data;
using satelliteBackend.Models.Dto;
using System.Text.Json;

public class LocationService  : ILocationService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;
    private readonly OpenMeteoForecastService _forecastService;

    public LocationService(HttpClient httpClient, ApplicationDbContext context, OpenMeteoForecastService forecastService)
    {
        _httpClient = httpClient;
        _context = context;
        _forecastService = forecastService;

    }

    public async Task<object> GetWeatherAndImagesByLocationId(int locationId)
    {
        var location = await _context.Locations.Include(l => l.WeatherForecasts)
            .FirstOrDefaultAsync(l => l.Id == locationId);

        if (location == null) return null;

        var weatherData = location.WeatherForecasts.Select(wf => new WeatherForecastDto
        {
            ForecastDate = wf.ForecastDate,
            Temperature = wf.Temperature,
            WindSpeed = wf.WindSpeed,
            Humidity = wf.Humidity,
            Precipitation = wf.Precipitation,
            UvIndex = wf.UvIndex,
            WeatherDescription = wf.WeatherDescription
        }).ToList();

        var pythonResponses = await _context.PythonResponses
            .Include(pr => pr.Location)
            .Where(pr => pr.LocationId == locationId)
            .ToListAsync();

        if (pythonResponses == null || pythonResponses.Count == 0)
            return null;

        var images = pythonResponses
            .SelectMany(pr => pr.Images, (pr, image) => new
            {
                pr.LocationId,
                pr.Location.SouthWestLatitude,
                pr.Location.SouthWestLongitude,
                pr.Location.NorthEastLatitude,
                pr.Location.NorthEastLongitude,
                Image = image
            })
            .ToList();

        return new
        {
            locationId = location.Id,
            weatherData = weatherData,
            images = images
        };
    }


     public async Task<List<WeatherForecastDto>> GetWeatherDataByLocationId(int locationId)
    {
        var location = await _context.Locations.Include(l => l.WeatherForecasts)
            .FirstOrDefaultAsync(l => l.Id == locationId);

        if (location == null) return null;

        // DTO sınıfını kullanarak dönüştürme işlemi
        return location.WeatherForecasts.Select(wf => new WeatherForecastDto
        {
            ForecastDate = wf.ForecastDate,
            Temperature = wf.Temperature,
            WindSpeed = wf.WindSpeed,
            Humidity = wf.Humidity,
            Precipitation = wf.Precipitation,
            UvIndex = wf.UvIndex,
            WeatherDescription = wf.WeatherDescription
        }).ToList();
    }

     public async Task<List<ImageDto>> GetImagesByLocationId(int locationId)
        {
            var pythonResponses = await _context.PythonResponses
                .Include(pr => pr.Location)
                .Where(pr => pr.LocationId == locationId)
                .ToListAsync();

            if (pythonResponses == null || pythonResponses.Count == 0)
                return null;

            // DTO sınıfını kullanarak dönüştürme işlemi
            return pythonResponses
                .SelectMany(pr => pr.Images, (pr, image) => new ImageDto
                {
                    LocationId = pr.LocationId,
                    SouthWestLatitude = pr.Location.SouthWestLatitude,
                    SouthWestLongitude = pr.Location.SouthWestLongitude,
                    NorthEastLatitude = pr.Location.NorthEastLatitude,
                    NorthEastLongitude = pr.Location.NorthEastLongitude,
                    Image = image
                })
                .ToList();
        }


    public async Task<LocationDetailsResponseDto?> GetWeatherAndAnalysesByLocationId(int locationId)
    {
        var location = await _context.Locations
            .Include(l => l.WeatherForecasts)
            .Include(l => l.Analyses)
            .FirstOrDefaultAsync(l => l.Id == locationId);

        if (location == null)
            return null;

        return new LocationDetailsResponseDto
        {
            LocationId = location.Id,

            WeatherForecasts = location.WeatherForecasts.Select(w => new WeatherForecastDto
            {
                ForecastDate = w.ForecastDate,
                Temperature = w.Temperature,
                WindSpeed = w.WindSpeed,
                Humidity = w.Humidity,
                Precipitation = w.Precipitation,
                UvIndex = w.UvIndex,
                WeatherDescription = w.WeatherDescription
            }).ToList(),

            Analyses = location.Analyses.Select(a => new AnalysisDto
            {
                Date = a.AnalysisDate.ToString("yyyy-MM-dd"),
                Average = a.AverageNDVI,
                Comment = a.Comment,
                Recommendation = a.Recommendation,
                Details = JsonSerializer.Deserialize<Dictionary<string, AnalysisDetail>>(a.DetailsJson)
            }).ToList()
        };
    }



    public async Task<List<LocationImage>> SendToPythonService(Location location)
    {
        var requestData = new
        {
            southWest = new { lat = location.SouthWestLatitude, lng = location.SouthWestLongitude },
            northEast = new { lat = location.NorthEastLatitude, lng = location.NorthEastLongitude },
            startDate = location.StartDate,
            endDate = location.EndDate
        };

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5001/process-location", requestData);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Hata Yanıtı: " + errorMessage);
            throw new Exception("Python servisine istek başarısız oldu.");
        }

        var result = await response.Content.ReadFromJsonAsync<PythonResponseDto>();
        if (result == null || result.Images == null || result.Images.Count == 0)
        {
            throw new Exception("Python servisi beklenen formatta veri döndürmedi.");
        }

        var locationImages = new List<LocationImage>();
        var baseDate = location.StartDate;

        for (int i = 0; i < result.Images.Count; i++)
        {
            var imageBase64 = result.Images[i];
            var imageType = i % 2 == 0 ? "RGB" : "NDVI"; // sıralamaya göre
            var imageDate = baseDate.AddDays(i / 2); // her 2 görsel 1 güne ait

            var locationImage = new LocationImage
            {
                LocationId = location.Id,
                ImageBase64 = imageBase64,
                ImageType = imageType,
                ImageDate = imageDate
            };

            _context.LocationImages.Add(locationImage);
            locationImages.Add(locationImage);
        }


        // 📊 Analiz verilerini kaydet
        if (result.Analyses != null && result.Analyses.Count > 0)
        {
            foreach (var analysis in result.Analyses)
            {
                var entity = new LocationAnalysis
                {
                    LocationId = location.Id,
                    AnalysisDate = DateTime.Parse(analysis.Date),
                    AverageNDVI = analysis.Average,
                    Comment = analysis.Comment ?? "Yorum yok",
                    Recommendation = analysis.Recommendation ?? "",
                    DetailsJson = JsonSerializer.Serialize(analysis.Details)
                };
                _context.LocationAnalyses.Add(entity);
            }
        }

        await _context.SaveChangesAsync();
        return locationImages;
    }


    public async Task<List<OpenMeteoForecastDto>> GetWeatherForecastForLocationAsync(Location location)
    {
        // Orta nokta hesapla
        var midLat = (location.SouthWestLatitude + location.NorthEastLatitude) / 2;
        var midLon = (location.SouthWestLongitude + location.NorthEastLongitude) / 2;

        // OpenMeteo servisini çağır
        var forecast = await _forecastService.GetHistoricalForecastAsync(
            midLat,
            midLon,
            location.StartDate,
            location.EndDate
        );

        return forecast;
    }


    public async Task<object> AddLocationAsync(LocationDto locationDto, int userId)
    {
        if (locationDto.StartDate.Date >= locationDto.EndDate.Date)
            return new { Success = false, Message = "Başlangıç tarihi bitiş tarihinden büyük veya eşit olamaz." };
      

        var location = new Location
        {
            SouthWestLatitude = locationDto.SouthWestLatitude,
            SouthWestLongitude = locationDto.SouthWestLongitude,
            NorthEastLatitude = locationDto.NorthEastLatitude,
            NorthEastLongitude = locationDto.NorthEastLongitude,
            StartDate = locationDto.StartDate,
            EndDate = locationDto.EndDate,
            Title = locationDto.Title,
            UserId = userId
        };

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var imagesBase64 = await SendToPythonService(location);
            if (imagesBase64 == null || imagesBase64.Count == 0)
            {
                await transaction.RollbackAsync();
                return new { Success = false, Message = "Görüntüler alınamadı." };
            }

            var pythonResponse = new PythonResponse
            {
                Images = imagesBase64.Select(i => i.ImageBase64).ToList(),
                LocationId = location.Id
            };
            _context.PythonResponses.Add(pythonResponse);

            var responseImages = imagesBase64.Select(image => new
            {
                locationId = location.Id,
                location.SouthWestLatitude,
                location.SouthWestLongitude,
                location.NorthEastLatitude,
                location.NorthEastLongitude,
                image = image.ImageBase64
            }).ToList();


            var forecasts = await GetWeatherForecastForLocationAsync(location);
            foreach (var forecast in forecasts)
            {
                var weather = new WeatherForecast
                {
                    LocationId = location.Id,
                    ForecastDate = forecast.Time,
                    Temperature = forecast.TemperatureC,
                    WindSpeed = forecast.WindSpeed,
                    Precipitation = forecast.Precipitation,
                    Humidity = forecast.Humidity,
                    UvIndex = forecast.UvIndexMax,
                    WeatherDescription = "Clear"
                };

                _context.WeatherForecasts.Add(weather);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new
            {
                locationId = location.Id,
                images = responseImages,
                weatherData = forecasts.Select(f => new
                {
                    f.Time,
                    f.TemperatureC,
                    f.WindSpeed,
                    f.Humidity,
                    f.Precipitation,
                    f.UvIndexMax
                })
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new { Success = false, Message = $"Bir hata oluştu: {ex.Message}" };
        }
    }


    public async Task<List<ImageDto>> GetUserImagesAsync(int userId)
    {
        var pythonResponses = await _context.PythonResponses
            .Include(pr => pr.Location)
            .Where(pr => pr.Location.UserId == userId)
            .ToListAsync();

        if (pythonResponses == null || pythonResponses.Count == 0)
            return null;

        var images = pythonResponses
            .SelectMany(pr => pr.Images, (pr, image) => new ImageDto
            {
                LocationId = pr.LocationId,
                SouthWestLatitude = pr.Location.SouthWestLatitude,
                SouthWestLongitude = pr.Location.SouthWestLongitude,
                NorthEastLatitude = pr.Location.NorthEastLatitude,
                NorthEastLongitude = pr.Location.NorthEastLongitude,
                Title = pr.Location.Title,
                Image = image
            })
            .ToList();

        return images;
    }


    public async Task<LocationDto> GetLocationByIdAsync(int locationId)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
        if (location == null) return null;

        return new LocationDto
        {
            
            SouthWestLatitude = location.SouthWestLatitude,
            SouthWestLongitude = location.SouthWestLongitude,
            NorthEastLatitude = location.NorthEastLatitude,
            NorthEastLongitude = location.NorthEastLongitude,
            StartDate = location.StartDate,
            EndDate = location.EndDate,
            Title = location.Title
        };
    }


    public async Task<object> DeleteLocationAsync(int locationId, int userId)
    {
        var location = await _context.Locations
            .Include(l => l.WeatherForecasts)
          
            .FirstOrDefaultAsync(l => l.Id == locationId && l.UserId == userId);

        if (location == null)
            return new { Success = false, Message = "Lokasyon bulunamadı veya yetkiniz yok." };

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.WeatherForecasts.RemoveRange(location.WeatherForecasts);
           
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new { Success = true, Message = "Lokasyon başarıyla silindi." };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new { Success = false, Message = $"Silme işlemi sırasında hata oluştu: {ex.Message}" };
        }
    }



}

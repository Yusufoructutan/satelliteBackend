using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreProject.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    // 1. Lokasyonun hava durumu ve görselleri
    [HttpGet("weather-and-images/{locationId}")]
    public async Task<IActionResult> GetWeatherAndImagesByLocationId(int locationId)
    {
        try
        {
            var result = await _locationService.GetWeatherAndImagesByLocationId(locationId);
            if (result == null)
            {
                return NotFound("Bu lokasyona ait veri bulunamadı.");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // 2. Lokasyonun hava durumu
    [HttpGet("weather/{locationId}")]
    public async Task<IActionResult> GetWeatherByLocationId(int locationId)
    {
        try
        {
            var weatherData = await _locationService.GetWeatherDataByLocationId(locationId);
            if (weatherData == null)
            {
                return NotFound("Belirtilen lokasyon bulunamadı.");
            }
            return Ok(weatherData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // 3. Lokasyonun görselleri
    [HttpGet("images/{locationId}")]
    public async Task<IActionResult> GetImagesByLocationId(int locationId)
    {
        try
        {
            var images = await _locationService.GetImagesByLocationId(locationId);
            if (images == null || images.Count == 0)
            {
                return NotFound("Bu lokasyona ait görseller bulunamadı.");
            }
            return Ok(images);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // 4. Lokasyon ekleme
    [HttpPost]
    public async Task<IActionResult> PostLocation([FromBody] LocationDto locationDto)
    {
        try
        {
            int userId = GetCurrentUserId(); // Kullanıcı ID'sini token'dan alıyoruz
            var result = await _locationService.AddLocationAsync(locationDto, userId);

            if (result is not null && result.GetType().GetProperty("Success")?.GetValue(result) as bool? == false)
            {
                return BadRequest(result.GetType().GetProperty("Message")?.GetValue(result));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // 5. Kullanıcıya ait görselleri getirme
    [HttpGet("user-images/{id}")]
    public async Task<IActionResult> GetUserImages(int id)
    {
        try
        {
            var images = await _locationService.GetUserImagesAsync(id);
            if (images == null || images.Count == 0)
            {
                return NotFound("Bu kullanıcıya ait görüntü bulunamadı.");
            }
            return Ok(images);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    // 6. Lokasyonu silme
    [HttpDelete("{locationId}")]
    public async Task<IActionResult> DeleteLocation(int locationId)
    {
        try
        {
            int userId = GetCurrentUserId(); // Kullanıcı ID'sini token'dan alıyoruz
            var result = await _locationService.DeleteLocationAsync(locationId, userId);

            if ((bool)result.GetType().GetProperty("Success")?.GetValue(result) == true)
                return Ok(result);

            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }
    // 8. Lokasyonun hava durumu ve NDVI analizlerini birlikte getirme
    [HttpGet("weather-and-analyses/{locationId}")]
    public async Task<IActionResult> GetWeatherAndAnalysesByLocationId(int locationId)
    {
        try
        {
            var result = await _locationService.GetWeatherAndAnalysesByLocationId(locationId);
            if (result == null)
            {
                return NotFound("Bu lokasyona ait hava durumu veya analiz verisi bulunamadı.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }


    // 7. Tekil Lokasyon Detayı
    [HttpGet("{locationId}")]
    public async Task<IActionResult> GetLocationById(int locationId)
    {
        try
        {
            var location = await _locationService.GetLocationByIdAsync(locationId);
            if (location == null)
                return NotFound(new { Message = "Lokasyon bulunamadı." });

            return Ok(location);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub") ??
                          User.FindFirst("userId") ??
                          User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");
        }

        return int.Parse(userIdClaim.Value);
    }

}

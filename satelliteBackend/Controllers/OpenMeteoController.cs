using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OpenMeteoController : ControllerBase
{
    private readonly OpenMeteoForecastService _forecastService;

    public OpenMeteoController(OpenMeteoForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecast(
        [FromQuery] double lat,
        [FromQuery] double lon,
        [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        var result = await _forecastService.GetHistoricalForecastAsync(lat, lon, start, end);
        return Ok(result);
    }
}

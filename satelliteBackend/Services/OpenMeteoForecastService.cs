using System.Globalization;
using System.Text.Json;
using System.Globalization;

public class OpenMeteoForecastService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoForecastService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

 public async Task<List<OpenMeteoForecastDto>> GetHistoricalForecastAsync(
    double latitude, double longitude, DateTime startDate, DateTime endDate)
{
    var url = $"https://archive-api.open-meteo.com/v1/archive" +
              $"?latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
              $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
              $"&daily=temperature_2m_max,temperature_2m_min,wind_speed_10m_max," +
              $"precipitation_sum,relative_humidity_2m_max,uv_index_max" + // Yeni veriler eklendi
              $"&start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}" +
              $"&timezone=Europe/Istanbul";

    var response = await _httpClient.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var json = await response.Content.ReadAsStringAsync();
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var apiResponse = JsonSerializer.Deserialize<OpenMeteoApiResponse>(json, options);

    var forecasts = new List<OpenMeteoForecastDto>();

    // Yeni verileri ekledik
    for (int i = 0; i < apiResponse.Daily.Time.Count; i++)
    {
        forecasts.Add(new OpenMeteoForecastDto
        {
            Time = DateTime.Parse(apiResponse.Daily.Time[i]),
            TemperatureC = apiResponse.Daily.Temperature_2m_Max[i] ?? 0.0,
            WindSpeed = apiResponse.Daily.Wind_Speed_10m_Max[i] ?? 0.0,
            Precipitation = apiResponse.Daily.Precipitation_Sum[i] ?? 0.0,  // Yağış
            HumidityMax = apiResponse.Daily.Relative_Humidity_2m_Max[i] ?? 0.0,  // Nem
            UvIndexMax = apiResponse.Daily.Uv_Index_Max[i] ?? 0.0  // UV Endeksi
        });
    }

    return forecasts;
}

}

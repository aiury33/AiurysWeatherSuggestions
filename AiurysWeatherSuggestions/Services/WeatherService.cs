using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services.Interfaces;
using Newtonsoft.Json;
using System.Globalization;

namespace AiurysWeatherSuggestions.Services;

public class WeatherService(HttpClient httpClient) : IWeatherService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<WeatherData?> GetWeatherAsync(double lat, double lon)
    {
        var latitude = lat.ToString("0.00", CultureInfo.InvariantCulture);
        var longitude = lon.ToString("0.00", CultureInfo.InvariantCulture);

        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m,relative_humidity_2m,precipitation_probability";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var weather = JsonConvert.DeserializeObject<WeatherData>(json);
        return weather;
    }
}

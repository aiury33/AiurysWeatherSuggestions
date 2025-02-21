using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services.Interfaces;
using Newtonsoft.Json;
using System.Globalization;

namespace AiurysWeatherSuggestions.Services
{
    public class WeatherService(HttpClient httpClient) : IWeatherService
    {
        private readonly HttpClient _httpClient = httpClient;
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        public async Task<WeatherData?> GetWeatherAsync(double lat, double lon)
        {
            string url = BuildWeatherApiUrl(lat, lon);
            string? jsonResponse = await GetApiResponseAsync(url);
            if (string.IsNullOrEmpty(jsonResponse)) return null;

            return ParseWeatherData(jsonResponse);
        }

        private static string BuildWeatherApiUrl(double lat, double lon)
        {
            string latitude = lat.ToString("0.00", CultureInfo.InvariantCulture);
            string longitude = lon.ToString("0.00", CultureInfo.InvariantCulture);

            return $"{BaseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m,relative_humidity_2m,precipitation_probability";
        }

        private async Task<string?> GetApiResponseAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsStringAsync();
        }

        private static WeatherData? ParseWeatherData(string json) => JsonConvert.DeserializeObject<WeatherData>(json);
    }
}

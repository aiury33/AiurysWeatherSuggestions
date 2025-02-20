using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace AiurysWeatherSuggestions.Services;

public class WeatherAdviceService(HttpClient httpClient) : IWeatherAdviceService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string GeminiApiKey = "{insert_your_key_here}";
    private const string GeminiApiUrlTemplate = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={0}";

    private readonly string _geminiApiUrl = string.Format(GeminiApiUrlTemplate, GeminiApiKey);

    public async Task<string> GetWeatherAdviceAsync(IpInfo location, WeatherData weather, string culture)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $"I am in {location.City}, {location.Country}. " +
                            $"The current temperature is {weather.Current.Temperature_2m}°C with a wind speed of {weather.Current.Wind_Speed_10m}m/s, " +
                            $"the precipitation probability is {weather.Current.Precipitation_Probability} and the relative humidity is {weather.Current.Relative_Humidity_2m}. " +
                            $"Answer in language {culture}: What activity suggestions do you have for today, considering details and places from my city and current weather? OBS: Send suggestions for 1) morning, 2) afternoon, 3) evening."
                        }
                    }
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_geminiApiUrl, content);
        if (!response.IsSuccessStatusCode)
            return "Could not retrieve health advice.";

        var json = await response.Content.ReadAsStringAsync();
        var parsedResponse = JsonConvert.DeserializeObject<GeminiResponse>(json);
        return parsedResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No advice received.";
    }
}

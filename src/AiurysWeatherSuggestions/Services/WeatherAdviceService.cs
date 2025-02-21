using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace AiurysWeatherSuggestions.Services
{
    public class WeatherAdviceService(HttpClient httpClient) : IWeatherAdviceService
    {
        private const string GeminiApiKey = "{insert_your_key_here}";
        private const string GeminiApiUrlTemplate = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={0}";
        private readonly string _geminiApiUrl = string.Format(GeminiApiUrlTemplate, GeminiApiKey);

        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> GetWeatherAdviceAsync(IpInfo location, WeatherData weather, string culture)
        {
            var requestBody = BuildRequestBody(location, weather, culture);
            var content = CreateHttpContent(requestBody);
            var responseJson = await PostRequestAsync(content);

            return ParseResponse(responseJson);
        }

        private static object BuildRequestBody(IpInfo location, WeatherData weather, string culture) => new
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

        private static StringContent CreateHttpContent(object requestBody)
        {
            string jsonString = JsonConvert.SerializeObject(requestBody);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        private async Task<string> PostRequestAsync(StringContent content)
        {
            var response = await _httpClient.PostAsync(_geminiApiUrl, content);
            if (!response.IsSuccessStatusCode) return string.Empty;

            return await response.Content.ReadAsStringAsync();
        }

        private static string ParseResponse(string json)
        {
            if (string.IsNullOrEmpty(json)) return "Could not retrieve health advice.";

            var parsedResponse = JsonConvert.DeserializeObject<GeminiResponse>(json);
            return parsedResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No advice received.";
        }
    }
}

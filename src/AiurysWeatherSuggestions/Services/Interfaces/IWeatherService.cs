using AiurysWeatherSuggestions.Models;
using System.Threading.Tasks;

namespace AiurysWeatherSuggestions.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherData?> GetWeatherAsync(double latitude, double longitude);
    }
}

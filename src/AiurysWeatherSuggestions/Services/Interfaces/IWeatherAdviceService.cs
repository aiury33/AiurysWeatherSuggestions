using AiurysWeatherSuggestions.Models;
using System.Runtime;
using System.Threading.Tasks;

namespace AiurysWeatherSuggestions.Services.Interfaces
{
    public interface IWeatherAdviceService
    {
        Task<string> GetWeatherAdviceAsync(IpInfo location, WeatherData weather, string culture);
    }
}

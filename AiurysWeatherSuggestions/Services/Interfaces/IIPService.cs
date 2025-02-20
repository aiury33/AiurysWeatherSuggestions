using AiurysWeatherSuggestions.Models;
using System.Runtime;
using System.Threading.Tasks;

namespace AiurysWeatherSuggestions.Services.Interfaces
{
    public interface IIPService
    {
        Task<string?> GetPublicIPAsync();
        Task<IpInfo?> GetLocationAsync(string ip);
    }
}

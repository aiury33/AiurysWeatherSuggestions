using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services.Interfaces;
using Newtonsoft.Json;

namespace AiurysWeatherSuggestions.Services;

public class IPService(HttpClient httpClient) : IIPService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string PublicIpUrl = "http://edns.ip-api.com/json";
    private const string LocationUrlTemplate = "http://ip-api.com/json/{0}";

    public async Task<string?> GetPublicIPAsync()
    {
        var response = await _httpClient.GetAsync(PublicIpUrl);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var ipResponse = JsonConvert.DeserializeObject<Root>(json);
        return ipResponse?.Dns?.Ip;
    }

    public async Task<IpInfo?> GetLocationAsync(string ip)
    {
        var url = string.Format(LocationUrlTemplate, ip);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var location = JsonConvert.DeserializeObject<IpInfo>(json);
        return location;
    }
}

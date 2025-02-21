using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services;
using AiurysWeatherSuggestions.Services.Interfaces;

namespace AiurysWeatherSuggestions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient<IIPService, IPService>();
        services.AddHttpClient<IWeatherService, WeatherService>();
        services.AddHttpClient<IWeatherAdviceService, WeatherAdviceService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to the Weather Advice API.");
            });

            endpoints.MapGet("/weatheradvice", async context =>
            {
                var latQuery = context.Request.Query["lat"].ToString();
                var lonQuery = context.Request.Query["lon"].ToString();
                var city = context.Request.Query["city"].ToString();
                var country = context.Request.Query["country"].ToString();
                var culture = context.Request.Query["culture"].ToString() ?? "en-us";

                if (string.IsNullOrEmpty(latQuery) || string.IsNullOrEmpty(lonQuery) ||
                    string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Please provide 'lat', 'lon', 'city', and 'country' query parameters.");
                    return;
                }

                if (!double.TryParse(latQuery, out double lat) || !double.TryParse(lonQuery, out double lon))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid latitude or longitude values.");
                    return;
                }

                var location = new IpInfo
                {
                    City = city,
                    Country = country,
                    Lat = lat,
                    Lon = lon
                };

                var weatherService = context.RequestServices.GetService<IWeatherService>();
                var weatherAdviceService = context.RequestServices.GetService<IWeatherAdviceService>();

                var weather = await weatherService.GetWeatherAsync(location.Lat, location.Lon);
                if (weather == null)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Could not retrieve weather.");
                    return;
                }

                var advice = await weatherAdviceService.GetWeatherAdviceAsync(location, weather, culture);
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(advice);
            });
        });
    }
}

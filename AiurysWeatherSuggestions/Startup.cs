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
                var ipService = context.RequestServices.GetService<IIPService>();
                var weatherService = context.RequestServices.GetService<IWeatherService>();
                var weatherAdviceService = context.RequestServices.GetService<IWeatherAdviceService>();

                var ip = await ipService.GetPublicIPAsync();
                if (string.IsNullOrEmpty(ip))
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Could not determine public IP.");
                    return;
                }

                var location = await ipService.GetLocationAsync(ip);
                if (location == null)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Could not determine location.");
                    return;
                }

                var weather = await weatherService.GetWeatherAsync(location.Lat, location.Lon);
                if (weather == null)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Could not retrieve weather.");
                    return;
                }

                var advice = await weatherAdviceService.GetWeatherAdviceAsync(location, weather, "EN-US");
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(advice);
            });
        });
    }
}

using AiurysWeatherSuggestions.Models;
using AiurysWeatherSuggestions.Services;

namespace AiurysWeatherSuggestions.IntegrationTests
{
    public class ServicesTests
    {
        public static IEnumerable<object[]> WeatherAdviceTestData =>
        [
            ["PT-BR", new IpInfo { City = "Belo Horizonte", Country = "Brazil", Lat = -19.91, Lon = 43.94 }],
            ["EN-US", new IpInfo { City = "Cleveland", Country = "United States", Lat = 41.50, Lon = -81.68 }],
            ["ES-ES", new IpInfo { City = "Barcelona", Country = "Spain", Lat = 41.39, Lon = 2.15 }]
        ];

        [Fact]
        public async void WeatherAdvice_ShouldProvideAdvice_BasedOnCurrentLocation()
        {
            // Arrange
            using var httpClient = new HttpClient();

            var ipService = new IPService(httpClient);
            var weatherService = new WeatherService(httpClient);
            var adviceService = new WeatherAdviceService(httpClient);

            // Act
            var myIp = await ipService.GetPublicIPAsync();
            var myLocation = await ipService.GetLocationAsync(myIp);
            var myWeather = await weatherService.GetWeatherAsync(myLocation.Lat, myLocation.Lon);
            var advice = await adviceService.GetWeatherAdviceAsync(myLocation, myWeather, "PT-BR");

            // Assert
            Assert.NotEmpty(advice);
        }

        [Theory]
        [MemberData(nameof(WeatherAdviceTestData))]
        public async Task WeatherAdvice_ShouldProvideAdvice_BasedOnPassedArguments(string culture, IpInfo location)
        {
            // Arrange
            using var httpClient = new HttpClient();
            var weatherService = new WeatherService(httpClient);
            var adviceService = new WeatherAdviceService(httpClient);

            // Act
            var myWeather = await weatherService.GetWeatherAsync(location.Lat, location.Lon);
            var advice = await adviceService.GetWeatherAdviceAsync(location, myWeather, culture);

            // Assert
            Assert.NotEmpty(advice);
        }
    }
}
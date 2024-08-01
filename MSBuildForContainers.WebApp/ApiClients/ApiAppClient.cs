using MSBuildForContainers.WebApp.Models;

namespace MSBuildForContainers.WebApp.ApiClients;

public interface IApiAppClient
{
    Task<List<WeatherForecast>> GetWeatherForecastAsync();
}

public class ApiAppClient(HttpClient http) : IApiAppClient
{
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    public async Task<List<WeatherForecast>> GetWeatherForecastAsync()
    {
        var forecasts = await _http.GetFromJsonAsync<List<WeatherForecast>>("weatherforecast").ConfigureAwait(false);

        return forecasts ?? [];
    }
}

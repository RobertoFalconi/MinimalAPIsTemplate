using MinimalAPIsAndCleanArchitecture.Core.Interfaces;
using MinimalAPIsAndCleanArchitecture.Core.Models;

namespace MinimalAPIsAndCleanArchitecture.Core.Services;

public class WeatherForecastService(IWeatherForecastRepository repository) : IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecast>> GetAllAsync() =>
        repository.GetAllAsync();

    public async Task<WeatherForecast> CreateAsync(CreateWeatherForecastRequest request)
    {
        var forecast = new WeatherForecast
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            Summary = request.Summary
        };

        return await repository.AddAsync(forecast);
    }
}

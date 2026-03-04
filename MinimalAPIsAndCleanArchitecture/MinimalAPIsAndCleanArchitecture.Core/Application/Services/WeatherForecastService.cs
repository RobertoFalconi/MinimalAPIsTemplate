using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.Interfaces;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Services;

public class WeatherForecastService(IWeatherForecastRepository repository) : IWeatherForecastService
{
    public Task<IEnumerable<WeatherForecast>> GetAllAsync() =>
        repository.GetAllAsync();

    public async Task<WeatherForecast> CreateAsync(CreateWeatherForecastCommand command)
    {
        var forecast = WeatherForecast.Create(command.Date, command.TemperatureC, command.Summary);
        return await repository.AddAsync(forecast);
    }
}

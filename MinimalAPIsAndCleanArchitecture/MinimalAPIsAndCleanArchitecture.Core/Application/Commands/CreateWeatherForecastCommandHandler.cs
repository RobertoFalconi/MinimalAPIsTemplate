using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Commands;

public class CreateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    : ICommandHandler<CreateWeatherForecastCommand, WeatherForecast>
{
    public async Task<WeatherForecast> HandleAsync(CreateWeatherForecastCommand command)
    {
        var forecast = WeatherForecast.Create(command.Date, command.TemperatureC, command.Summary);
        return await repository.AddAsync(forecast);
    }
}

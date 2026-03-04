using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Commands;

public class CreateWeatherForecastCommandHandler(IWeatherForecastRepository repository)
    : ICommandHandler<CreateWeatherForecastCommand, WeatherForecastResponse>
{
    public async Task<WeatherForecastResponse> HandleAsync(CreateWeatherForecastCommand command)
    {
        var forecast = WeatherForecast.Create(command.Date, command.TemperatureC, command.Summary);
        var created = await repository.AddAsync(forecast);
        return new WeatherForecastResponse(created.Id, created.Date, created.TemperatureC, created.TemperatureF, created.Summary);
    }
}

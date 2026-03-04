using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Interfaces;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast> CreateAsync(CreateWeatherForecastCommand command);
}

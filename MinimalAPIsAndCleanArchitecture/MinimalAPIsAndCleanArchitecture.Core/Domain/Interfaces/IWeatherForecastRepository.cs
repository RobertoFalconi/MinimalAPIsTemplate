using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;

namespace MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

public interface IWeatherForecastRepository
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast> AddAsync(WeatherForecast forecast);
}

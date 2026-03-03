using MinimalAPIsAndCleanArchitecture.Core.Models;

namespace MinimalAPIsAndCleanArchitecture.Core.Interfaces;

public interface IWeatherForecastRepository
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast> AddAsync(WeatherForecast forecast);
}

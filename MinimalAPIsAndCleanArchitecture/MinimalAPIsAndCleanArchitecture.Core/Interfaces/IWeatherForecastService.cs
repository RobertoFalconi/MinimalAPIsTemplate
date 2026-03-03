using MinimalAPIsAndCleanArchitecture.Core.Models;

namespace MinimalAPIsAndCleanArchitecture.Core.Interfaces;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
    Task<WeatherForecast> CreateAsync(CreateWeatherForecastRequest request);
}

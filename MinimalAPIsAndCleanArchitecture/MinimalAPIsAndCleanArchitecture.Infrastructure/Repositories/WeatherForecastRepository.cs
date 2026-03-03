using Microsoft.EntityFrameworkCore;
using MinimalAPIsAndCleanArchitecture.Core.Interfaces;
using MinimalAPIsAndCleanArchitecture.Core.Models;
using MinimalAPIsAndCleanArchitecture.Infrastructure.Data;

namespace MinimalAPIsAndCleanArchitecture.Infrastucture.Repositories;

public class WeatherForecastRepository(AppDbContext context) : IWeatherForecastRepository
{
    public async Task<IEnumerable<WeatherForecast>> GetAllAsync()
        => await context.WeatherForecasts.ToListAsync();

    public async Task<WeatherForecast> AddAsync(WeatherForecast forecast)
    {
        context.WeatherForecasts.Add(forecast);
        await context.SaveChangesAsync();
        return forecast;
    }
}

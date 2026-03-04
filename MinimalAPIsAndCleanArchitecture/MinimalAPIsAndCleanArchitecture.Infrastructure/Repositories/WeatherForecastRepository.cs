using Microsoft.EntityFrameworkCore;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;
using MinimalAPIsAndCleanArchitecture.Infrastructure.Data;

namespace MinimalAPIsAndCleanArchitecture.Infrastructure.Repositories;

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

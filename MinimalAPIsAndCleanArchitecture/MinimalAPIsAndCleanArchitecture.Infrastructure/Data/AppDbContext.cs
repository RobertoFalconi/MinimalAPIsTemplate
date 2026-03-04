using Microsoft.EntityFrameworkCore;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;

namespace MinimalAPIsAndCleanArchitecture.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>()
            .Ignore(w => w.TemperatureF);
    }
}

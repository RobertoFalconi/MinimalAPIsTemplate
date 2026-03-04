using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;
using MinimalAPIsAndCleanArchitecture.Infrastructure.Data;
using MinimalAPIsAndCleanArchitecture.Infrastructure.Repositories;

namespace MinimalAPIsAndCleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
        return services;
    }
}

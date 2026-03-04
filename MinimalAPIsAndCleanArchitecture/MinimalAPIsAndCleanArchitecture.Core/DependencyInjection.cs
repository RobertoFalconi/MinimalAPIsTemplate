using Microsoft.Extensions.DependencyInjection;
using MinimalAPIsAndCleanArchitecture.Core.Application.Interfaces;
using MinimalAPIsAndCleanArchitecture.Core.Application.Services;

namespace MinimalAPIsAndCleanArchitecture.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        return services;
    }
}

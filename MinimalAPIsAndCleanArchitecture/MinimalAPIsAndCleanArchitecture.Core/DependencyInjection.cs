using Microsoft.Extensions.DependencyInjection;
using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;
using MinimalAPIsAndCleanArchitecture.Core.Application.Queries;

namespace MinimalAPIsAndCleanArchitecture.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<
            IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecastResponse>>,
            GetAllWeatherForecastsQueryHandler>();

        services.AddScoped<
            ICommandHandler<CreateWeatherForecastCommand, WeatherForecastResponse>,
            CreateWeatherForecastCommandHandler>();

        return services;
    }
}


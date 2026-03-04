using Microsoft.Extensions.DependencyInjection;
using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.Queries;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;

namespace MinimalAPIsAndCleanArchitecture.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<
            IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecast>>,
            GetAllWeatherForecastsQueryHandler>();

        services.AddScoped<
            ICommandHandler<CreateWeatherForecastCommand, WeatherForecast>,
            CreateWeatherForecastCommandHandler>();

        return services;
    }
}


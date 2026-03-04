using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;
using MinimalAPIsAndCleanArchitecture.Core.Application.Queries;

namespace MinimalAPIsAndCleanArchitecture.Endpoints;

internal static class WeatherForecastEndpoint
{
    public static void MapWeatherForecast(this WebApplication app)
    {
        app.MapGet("/weatherforecast",
            async (IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecastResponse>> handler) =>
            {
                var result = await handler.HandleAsync(new GetAllWeatherForecastsQuery());
                return Results.Ok(result);
            })
            .WithName("GetWeatherForecast");

        app.MapPost("/weatherforecast",
            async (CreateWeatherForecastCommand command,
                   ICommandHandler<CreateWeatherForecastCommand, WeatherForecastResponse> handler) =>
            {
                var created = await handler.HandleAsync(command);
                return Results.Created($"/weatherforecast/{created.Id}", created);
            })
            .WithName("CreateWeatherForecast");
    }
}

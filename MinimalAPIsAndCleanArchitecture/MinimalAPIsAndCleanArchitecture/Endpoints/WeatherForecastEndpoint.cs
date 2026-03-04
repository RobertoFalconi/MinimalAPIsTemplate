using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Endpoints;

internal static class WeatherForecastEndpoint
{
    public static void MapWeatherForecast(this WebApplication app)
    {
        app.MapGet("/weatherforecast", async (IWeatherForecastService service) =>
        {
            var forecasts = await service.GetAllAsync();
            return Results.Ok(forecasts);
        })
        .WithName("GetWeatherForecast");

        app.MapPost("/weatherforecast", async (CreateWeatherForecastCommand command, IWeatherForecastService service) =>
        {
            var created = await service.CreateAsync(command);
            return Results.Created($"/weatherforecast/{created.Id}", created);
        })
        .WithName("CreateWeatherForecast");
    }
}
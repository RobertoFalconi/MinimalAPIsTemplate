using MinimalAPIsAndCleanArchitecture.Core.Interfaces;
using MinimalAPIsAndCleanArchitecture.Core.Models;

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

        app.MapPost("/weatherforecast", async (CreateWeatherForecastRequest request, IWeatherForecastService service) =>
        {
            var created = await service.CreateAsync(request);
            return Results.Created($"/weatherforecast/{created.Id}", created);
        })
        .WithName("CreateWeatherForecast");
    }
}
using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Queries;

public class GetAllWeatherForecastsQueryHandler(IWeatherForecastRepository repository)
    : IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecastResponse>>
{
    public async Task<IEnumerable<WeatherForecastResponse>> HandleAsync(GetAllWeatherForecastsQuery query)
    {
        var forecasts = await repository.GetAllAsync();
        return forecasts.Select(f => new WeatherForecastResponse(f.Id, f.Date, f.TemperatureC, f.TemperatureF, f.Summary));
    }
}

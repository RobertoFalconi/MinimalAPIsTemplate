using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;
using MinimalAPIsAndCleanArchitecture.Core.Domain.Interfaces;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Queries;

public class GetAllWeatherForecastsQueryHandler(IWeatherForecastRepository repository)
    : IQueryHandler<GetAllWeatherForecastsQuery, IEnumerable<WeatherForecast>>
{
    public Task<IEnumerable<WeatherForecast>> HandleAsync(GetAllWeatherForecastsQuery query) =>
        repository.GetAllAsync();
}

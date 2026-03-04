namespace MinimalAPIsAndCleanArchitecture.Core.Application.Commands;

public record CreateWeatherForecastCommand(DateOnly Date, int TemperatureC, string? Summary);

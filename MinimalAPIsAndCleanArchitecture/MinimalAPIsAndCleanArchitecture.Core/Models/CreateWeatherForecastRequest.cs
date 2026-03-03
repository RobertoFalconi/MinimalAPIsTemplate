namespace MinimalAPIsAndCleanArchitecture.Core.Models;

public record CreateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);

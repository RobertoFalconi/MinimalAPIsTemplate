namespace MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;

public record WeatherForecastResponse(int Id, DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);

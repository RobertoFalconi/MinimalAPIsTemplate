namespace MinimalAPIsAndCleanArchitecture.Core.Domain.Entities;

public class WeatherForecast
{
    private WeatherForecast() { }

    public int Id { get; private set; }
    public DateOnly Date { get; private set; }
    public int TemperatureC { get; private set; }
    public string? Summary { get; private set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static WeatherForecast Create(DateOnly date, int temperatureC, string? summary)
    {
        return new WeatherForecast
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary
        };
    }
}

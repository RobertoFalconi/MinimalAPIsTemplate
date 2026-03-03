namespace MinimalAPIs.Models.API;

public record WeatherForecastAPI(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF
    {
        get
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    }
}
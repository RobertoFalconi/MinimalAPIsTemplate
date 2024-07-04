namespace Microsoft.Extensions.Logging;

public static class LoggerExtensions
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static void LogItem<T>(this ILogger logger, LogLevel logLevel, T data)
    {
        logger.Log(logLevel, JsonSerializer.Serialize(data, options));
    }

    public static void LogItemInformation<T>(this ILogger logger, T data)
    {
        logger.Log(LogLevel.Information, JsonSerializer.Serialize(data, options));
    }

    public static void LogItemWarning<T>(this ILogger logger, T data)
    {
        logger.Log(LogLevel.Warning, JsonSerializer.Serialize(data, options));
    }

    public static void LogItemError<T>(this ILogger logger, Exception exception, T? data)
    {
        logger.Log(LogLevel.Error, exception, JsonSerializer.Serialize(data, options));
    }
}

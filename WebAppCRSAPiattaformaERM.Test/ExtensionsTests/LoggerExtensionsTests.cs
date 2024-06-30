using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAppCRSAPiattaformaERM.Test.ExtensionsTests;

public class LoggerExtensionsTests
{
    private readonly Mock<ILogger> _mockLogger;

    public LoggerExtensionsTests()
    {
        _mockLogger = new Mock<ILogger>();
    }

    [Fact]
    public void LogItem_ShouldLogInformation()
    {
        // Arrange
        var data = new { Name = "Test" };
        var expectedLogMessage = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // Act
        _mockLogger.Object.LogItem(LogLevel.Information, data);

        // Assert
        _mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedLogMessage),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public void LogItemInformation_ShouldLogInformation()
    {
        // Arrange
        var data = new { Name = "Test" };
        var expectedLogMessage = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // Act
        _mockLogger.Object.LogItemInformation(data);

        // Assert
        _mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedLogMessage),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public void LogItemWarning_ShouldLogWarning()
    {
        // Arrange
        var data = new { Name = "Test" };
        var expectedLogMessage = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // Act
        _mockLogger.Object.LogItemWarning(data);

        // Assert
        _mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedLogMessage),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public void LogItemError_ShouldLogError()
    {
        // Arrange
        var data = new { Name = "Test" };
        var exception = new Exception("Test exception");
        var expectedLogMessage = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        // Act
        _mockLogger.Object.LogItemError(exception, data);

        // Assert
        _mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == expectedLogMessage),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
}

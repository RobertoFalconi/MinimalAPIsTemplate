using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebAppCRSAPiattaformaERM.Extensions;
using WebAppCRSAPiattaformaERM.Models.DB;

namespace WebAppCRSAPiattaformaERM.Test;

public class AppFixture : IDisposable
{
    public AppFixture()
    {
        Fixture = new();
        Fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));
    }

    internal IMediator Mediator
    {
        get
        {
            return new Mock<IMediator>().Object;
        }
    }

    internal IMapper Mapper
    {
        get
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfileExtensions>();
            });
                
            return config.CreateMapper();
        }
    }

    internal ILogger<T> Logger<T>()
    {
        var logger = new Mock<ILogger<T>>();

        logger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()))
            .Callback(new InvocationAction(invocation =>
            {
                var logLevel = (LogLevel)invocation.Arguments[0]; // The first two will always be whatever is specified in the setup above
                var state = invocation.Arguments[2];
                var exception = (Exception)invocation.Arguments[3];
                var formatter = invocation.Arguments[4];

                var invokeMethod = formatter.GetType().GetMethod("Invoke");
                var logMessage = invokeMethod?.Invoke(formatter, new[] { state, exception });

                Trace.WriteLine($"{logLevel} - {logMessage}");
            }));
        return logger.Object;
    }

    internal DS06017_crsa_questionarioContext DbContext
    {
        get
        {
            var options = new DbContextOptionsBuilder<DS06017_crsa_questionarioContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            DS06017_crsa_questionarioContext context = new(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }

    internal HttpClient HttpClientApi
    {
        get
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                    )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"[
                        {
                            ""id"": ""1"",
                            ""application"": ""App1"",
                            ""feedBack"": ""Positive feedback"",
                            ""rate"": 5,
                            ""createDateTime"": ""2024-01-31T12:30:00"",
                            ""url"": ""https://example.com"",
                            ""hash"": ""abcdef""
                        },
                        {
                            ""id"": ""2"",
                            ""application"": ""App2"",
                            ""feedBack"": ""Negative feedback"",
                            ""rate"": 2,
                            ""createDateTime"": ""2024-01-31T14:45:00"",
                            ""url"": ""https://example.net"",
                            ""hash"": ""123456""
                        },
                        {
                            ""id"": ""3"",
                            ""application"": ""App3"",
                            ""feedBack"": ""Neutral feedback"",
                            ""rate"": 3,
                            ""createDateTime"": ""2024-01-31T16:00:00"",
                            ""url"": null,
                            ""hash"": ""789xyz""
                        },
                        {
                            ""id"": ""4"",
                            ""application"": ""App3"",
                            ""feedBack"": """",
                            ""rate"": 3,
                            ""createDateTime"": ""2024-01-31T16:00:00"",
                            ""url"": null,
                            ""hash"": ""789xyz""
                        },
                        {
                            ""id"": ""5"",
                            ""application"": ""App3"",
                            ""feedBack"": """",
                            ""rate"": 1,
                            ""createDateTime"": ""2024-01-31T16:00:00"",
                            ""url"": null,
                            ""hash"": ""789xyz""
                        }
                        ]", Encoding.UTF8, "application/json")
                });

            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient("TestClient")
                .ConfigurePrimaryHttpMessageHandler(() => handlerMock.Object);

            HttpClient httpClient =
                services
                .BuildServiceProvider()
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient("TestClient");

            return httpClient;
        }
    }

    public static ILogger<T> GetLogger<T>()
    {
        var logger = new Mock<ILogger<T>>();

        logger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
            .Callback(new InvocationAction(invocation =>
            {
                var logLevel = (LogLevel)invocation.Arguments[0]; // The first two will always be whatever is specified in the setup above
                var state = invocation.Arguments[2];
                var exception = (Exception)invocation.Arguments[3];
                var formatter = invocation.Arguments[4];

                var invokeMethod = formatter.GetType().GetMethod("Invoke");
                var logMessage = invokeMethod?.Invoke(formatter, new[] { state, exception });

                Trace.WriteLine($"{logLevel} - {logMessage}");
            }));
        return logger.Object;
    }

    internal Fixture Fixture { get; private set; }

    public void Dispose()
    {
    }
}

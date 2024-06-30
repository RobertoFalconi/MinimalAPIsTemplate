using System.Diagnostics;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using WebAppCRSAPiattaformaERM.Pages;
using WebAppCRSAPiattaformaERM.Test;
using Xunit;

namespace WebAppCRSAPiattaformaERM.Test.PagesTests;

public class ErrorModelTests
{

    [Fact]
    public void OnGet_SetsRequestId()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorModel>>();

        // Simulate a HttpContext with TraceIdentifier

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.RequestServices)
                       .Returns(new ServiceCollection()
                       .BuildServiceProvider());

        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = httpContextMock.Object.RequestServices;

        // Act
        var errorModel = new ErrorModel(loggerMock.Object, httpContext);

        errorModel.OnGet();

        // Assert
        Assert.True(errorModel.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_ReturnsFalse_WhenRequestIdIsNullOrEmpty()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(loggerMock.Object);

        // Act & Assert
        errorModel.RequestId = null;
        Assert.False(errorModel.ShowRequestId);

        errorModel.RequestId = string.Empty;
        Assert.False(errorModel.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_ReturnsTrue_WhenRequestIdIsNotNullOrEmpty()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorModel>>();
        var errorModel = new ErrorModel(loggerMock.Object);

        // Act
        errorModel.RequestId = "NotEmptyRequestId";

        // Assert
        Assert.True(errorModel.ShowRequestId);
    }
}

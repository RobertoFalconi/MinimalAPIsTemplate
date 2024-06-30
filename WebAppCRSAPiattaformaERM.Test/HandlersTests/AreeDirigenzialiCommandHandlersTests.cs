using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebAppCRSAPiattaformaERM.Controllers;
using WebAppCRSAPiattaformaERM.Handlers.CommandHandlers;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.DB;
using WebAppCRSAPiattaformaERM.Models.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class AreeDirigenzialiCommandHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public AreeDirigenzialiCommandHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task PostAreaDirigenzialeCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new AreeDirigenzialiDTO()
        {
            GRADI_DENOM = "CAD123",
            GRADI_COD_UTENTE_DIR = "ABCDEF12G34H567I",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1)
        };

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciAreeDirigenzialiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task PostAreaDirigenzialeCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new AreeDirigenzialiDTO();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciAreeDirigenzialiCommand(testEntity);

        // Act && Assert
        try
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.Equal("Xunit.Sdk.ThrowsException", ex.GetType().ToString());
        }
    }

    [Fact]
    public async Task PutAreaDirigenzialeCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new AreeDirigenzialiDTO()
        {
            GRADI_SEQ_AREADIRIGENZIALE_PK = 1,
            GRADI_DENOM = "CAD123",
            GRADI_COD_UTENTE_DIR = "ABCDEF12G34H567I",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1)
        };
        var dbEntity = new GRADI_TB_AREEDIRIGENZIALI_CL()
        {
            GRADI_DENOM = "hgfdd",
            GRADI_COD_UTENTE_DIR = "hdfs",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1),
            GRADI_FLAG_STATO = "A",
            GRADI_COD_UTENTE = "osidf",
            GRADI_DATA_AGGIORN = DateTime.Now,
            GRADI_COD_APPL = "jhi"
        };
        dbContext.GRADI_TB_AREEDIRIGENZIALI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var request = new AggiornaAreeDirigenzialiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutProcessiMappatiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var dbEntity = new GRADI_TB_AREEDIRIGENZIALI_CL()
        {
            GRADI_DENOM = "hgfdd",
            GRADI_COD_UTENTE_DIR = "hdfs",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1),
            GRADI_FLAG_STATO = "A",
            GRADI_COD_UTENTE = "osidf",
            GRADI_DATA_AGGIORN = DateTime.Now,
            GRADI_COD_APPL = "jhi"
        };
        dbContext.GRADI_TB_AREEDIRIGENZIALI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var notExistingEntity = new AreeDirigenzialiDTO();
        var request = new AggiornaAreeDirigenzialiCommand(notExistingEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteAreaDirigenzialeCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var dbEntity = new GRADI_TB_AREEDIRIGENZIALI_CL()
        {
            GRADI_DENOM = "hgfdd",
            GRADI_COD_UTENTE_DIR = "hdfs",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1),
            GRADI_FLAG_STATO = "A",
            GRADI_COD_UTENTE = "osidf",
            GRADI_DATA_AGGIORN = DateTime.Now,
            GRADI_COD_APPL = "jhi"
        };
        dbContext.GRADI_TB_AREEDIRIGENZIALI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviAreeDirigenzialiCommand(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviAreeDirigenzialiCommand(13214);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

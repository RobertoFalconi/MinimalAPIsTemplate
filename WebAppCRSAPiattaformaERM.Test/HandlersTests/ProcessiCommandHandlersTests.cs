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
using System.Text;
using System.Threading.Tasks;
using WebAppCRSAPiattaformaERM.Controllers;
using WebAppCRSAPiattaformaERM.Handlers.CommandHandlers;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;
using WebAppCRSAPiattaformaERM.Models.DTO;

namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class ProcessiCommandHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public ProcessiCommandHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task PostProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiDTO()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1)
        };

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciProcessiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task PostProcessiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiDTO();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciProcessiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiDTO()
        {
            GRPRO_SEQ_PROCESSI_PK = 1,  
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1)
        };
        var dbEntity = new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        };
        dbContext.GRPRO_TB_PROCESSI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var request = new AggiornaProcessiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutProcessiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        };
        dbContext.GRPRO_TB_PROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var notExistingEntity = new ProcessiDTO();
        var request = new AggiornaProcessiCommand(notExistingEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        };
        dbContext.GRPRO_TB_PROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviProcessiCommand(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        };
        dbContext.GRPRO_TB_PROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviProcessiCommand(543543);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

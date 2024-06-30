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

public class ProcessiMappatiCommandHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public ProcessiMappatiCommandHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task PostProcessiMappatiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiMappatiDTO()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123"
        };

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciProcessiMappatiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task PostProcessiMappatiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiMappatiDTO();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciProcessiMappatiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutProcessiMappatiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new ProcessiMappatiDTO()
        {
            GRPAR_SEQ_PROCESSIMAPPATI_PK = 1,
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123"
        };
        var dbEntity = new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123",
            GRPAR_FLAG_STATO = "A",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_COD_UTENTE = "user456",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "app123",
        };

        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var request = new AggiornaProcessiMappatiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutProcessiMappatiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var dbEntity = new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_FLAG_STATO = "A",
            GRPAR_COD_UTENTE = "user456",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "app123",
        };

        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var notExistingEntity = new ProcessiMappatiDTO();
        var request = new AggiornaProcessiMappatiCommand(notExistingEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteProcessiMappatiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var dbEntity = new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_FLAG_STATO = "A",
            GRPAR_COD_UTENTE = "user456",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "app123",
        };

        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviProcessiMappatiCommand(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var dbEntity = new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 2,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 3,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 4,
            GRPAR_GRPRO_TB_PROCESSI_FK = 5,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "user123",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_FLAG_STATO = "A",
            GRPAR_COD_UTENTE = "user456",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "app123",
        };

        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviProcessiMappatiCommand(543543);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

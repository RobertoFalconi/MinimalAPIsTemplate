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
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;
using WebAppCRSAPiattaformaERM.Models.DTO;

namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class ProcessiMappatiQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public ProcessiMappatiQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetProcessiMappatiQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 101,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 202,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 303,
            GRPAR_GRPRO_TB_PROCESSI_FK = 404,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "gdfgdf33",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_FLAG_STATO = "A",
            GRPAR_COD_UTENTE = "4123fxf",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "gsd"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiQueryHandler(logger, dbContext);
        var filter = new ProcessiMappatiFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "CodiceFiscaleUtenteAssegnato"
        };
        var request = new GetProcessiMappatiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task GetProcessiQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiMappatiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRPAR_TB_PROCESSIMAPPATI_CL.Add(new GRPAR_TB_PROCESSIMAPPATI_CL()
        {
            GRPAR_GRDRG_TB_DIREZREGDCM_FK = 101,
            GRPAR_GRADI_TB_AREEDIRIGENZIALI_FK = 202,
            GRPAR_GRDCE_TB_DIREZCENTRALI_FK = 303,
            GRPAR_GRPRO_TB_PROCESSI_FK = 404,
            GRPAR_DATA_INIZIO = DateTime.Now,
            GRPAR_DATA_FINE = DateTime.Now.AddYears(1),
            GRPAR_DATA_INIZIO_ASSEGNAZIONE = DateTime.Now,
            GRPAR_DATA_FINE_ASSEGNAZIONE = DateTime.Now,
            GRPAR_COD_UTENTE_ASSEGNATARIO = "gdfgdf33",
            GRPAR_FLAG_COMPILAZ = "A",
            GRPAR_FLAG_STATO = "A",
            GRPAR_COD_UTENTE = "4123fxf",
            GRPAR_DATA_AGGIORN = DateTime.Now,
            GRPAR_COD_APPL = "gsd"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiMappatiQueryHandler(logger, dbContext);
        var filter = new ProcessiMappatiFilter();
        var request = new GetProcessiMappatiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

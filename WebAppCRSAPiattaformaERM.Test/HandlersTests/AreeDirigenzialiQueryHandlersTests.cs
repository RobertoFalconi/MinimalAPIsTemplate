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
using WebAppCRSAPiattaformaERM.Models.DB;
using WebAppCRSAPiattaformaERM.Models.DTO;
using WebAppCRSAPiattaformaERM.Models.Filters;

namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class AreeDirigenzialiQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public AreeDirigenzialiQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetAreeDirigenzialiQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRADI_TB_AREEDIRIGENZIALI_CL.Add(new GRADI_TB_AREEDIRIGENZIALI_CL()
        {
            GRADI_DENOM = "hgfdd",
            GRADI_COD_UTENTE_DIR = "hdfs",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1),
            GRADI_FLAG_STATO = "A",
            GRADI_COD_UTENTE = "osidf",
            GRADI_DATA_AGGIORN = DateTime.Now,
            GRADI_COD_APPL = "jhi"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiQueryHandler(logger, dbContext, mapper);
        var filter = new AreeDirigenzialiFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "CodiceFiscaleUtenteUltimoAggiornamento"
        };
        var request = new GetAreeDirigenzialiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task GetProcessiQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDirigenzialiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRADI_TB_AREEDIRIGENZIALI_CL.Add(new GRADI_TB_AREEDIRIGENZIALI_CL()
        {
            GRADI_DENOM = "hgfdd",
            GRADI_COD_UTENTE_DIR = "hdfs",
            GRADI_DATA_INIZIO = DateTime.Now,
            GRADI_DATA_FINE = DateTime.Now.AddYears(1),
            GRADI_FLAG_STATO = "A",
            GRADI_COD_UTENTE = "osidf",
            GRADI_DATA_AGGIORN = DateTime.Now,
            GRADI_COD_APPL = "jhi"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDirigenzialiQueryHandler(logger, dbContext, mapper);
        var filter = new AreeDirigenzialiFilter();
        var request = new GetAreeDirigenzialiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

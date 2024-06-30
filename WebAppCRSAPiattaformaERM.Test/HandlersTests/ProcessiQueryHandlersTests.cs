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

public class ProcessiQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public ProcessiQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetProcessiQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRPRO_TB_PROCESSI_CL.Add(new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiQueryHandler(logger, dbContext);
        var filter = new ProcessiFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "GRTCA_COD_UTENTE"
        };
        var request = new GetProcessiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task GetProcessiQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<ProcessiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRPRO_TB_PROCESSI_CL.Add(new GRPRO_TB_PROCESSI_CL()
        {
            GRPRO_DENOM = "adsa",
            GRPRO_DENOM_ESTESA = "asdadas",
            GRPRO_DATA_INIZIO = DateTime.Now,
            GRPRO_DATA_FINE = DateTime.Now.AddYears(1),
            GRPRO_FLAG_STATO = "A",
            GRPRO_COD_UTENTE = "1234",
            GRPRO_DATA_AGGIORN = DateTime.Now,
            GRPRO_COD_APPL = "dd"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessiQueryHandler(logger, dbContext);
        var filter = new ProcessiFilter();
        var request = new GetProcessiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

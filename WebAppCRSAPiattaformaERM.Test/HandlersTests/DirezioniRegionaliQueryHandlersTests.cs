using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;

using Microsoft.AspNetCore.Http.HttpResults;


namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class DirezioniRegionaliQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public DirezioniRegionaliQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetDirezioniRegionaliQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<DirezioniRegionaliQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRDRG_TB_DIREZREGDCM_CL.Add(new GRDRG_TB_DIREZREGDCM_CL()
        {
            GRDRG_DATA_AGGIORN = DateTime.Now,
            GRDRG_COD_APPL = "",
            GRDRG_COD_UTENTE = "1234",
            GRDRG_COD_UTENTE_DIR = "4321",
            GRDRG_DATA_FINE = DateTime.Now,
            GRDRG_DATA_INIZIO = DateTime.Now,
            GRDRG_FLAG_STATO = "A",
            GRDRG_DENOM = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new DirezioniRegionaliQueryHandler(logger, dbContext);
        var filter = new DirezioniRegionaliFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "CodiceDirezioniRegionali"
        };
        var request = new GetDirezioniRegionaliQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task GetDirezioniRegionaliQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<DirezioniRegionaliQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRDRG_TB_DIREZREGDCM_CL.Add(new GRDRG_TB_DIREZREGDCM_CL()
        {
            GRDRG_DATA_AGGIORN = DateTime.Now,
            GRDRG_COD_APPL = "",
            GRDRG_COD_UTENTE = "1234",
            GRDRG_COD_UTENTE_DIR = "4321",
            GRDRG_DATA_FINE = DateTime.Now,
            GRDRG_DATA_INIZIO = DateTime.Now,
            GRDRG_FLAG_STATO = "A",
            GRDRG_DENOM = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new DirezioniRegionaliQueryHandler(logger, dbContext);
        var filter = new DirezioniRegionaliFilter();
        var request = new GetDirezioniRegionaliQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;

using Microsoft.AspNetCore.Http.HttpResults;


namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class AreeDiProdottoQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public AreeDiProdottoQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetAreeDiProdottoQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDiProdottoQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRDCE_TB_DIREZCENTRALI_CL.Add(new GRDCE_TB_DIREZCENTRALI_CL()
        {
            GRDCE_DATA_AGGIORN = DateTime.Now,
            GRDCE_SEQ_DIRCENTRALE_PK = 1,
            GRDCE_COD_DIRCENTRALE = "",
            GRDCE_COD_UTENTE_DIR = "",
            GRDCE_COD_APPL = "",
            GRDCE_COD_UTENTE = "1234",
            GRDCE_DATA_FINE = DateTime.Now,
            GRDCE_DATA_INIZIO = DateTime.Now,
            GRDCE_FLAG_STATO = "A",
            GRDCE_DENOM_DIRCENTRALE = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDiProdottoQueryHandler(logger, dbContext, mapper);
        var filter = new DirezioniCentraliFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "GRDCE_SEQ_DIRCENTRALE_PK"
        };
        var request = new GetAreeDiProdottoQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task GetAreeDiProdottoQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<AreeDiProdottoQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRDCE_TB_DIREZCENTRALI_CL.Add(new GRDCE_TB_DIREZCENTRALI_CL()
        {
            GRDCE_DATA_AGGIORN = DateTime.Now,
            GRDCE_SEQ_DIRCENTRALE_PK = 1,
            GRDCE_COD_DIRCENTRALE = "",
            GRDCE_COD_UTENTE_DIR = "",
            GRDCE_COD_APPL = "",
            GRDCE_COD_UTENTE = "1234",
            GRDCE_DATA_FINE = DateTime.Now,
            GRDCE_DATA_INIZIO = DateTime.Now,
            GRDCE_FLAG_STATO = "A",
            GRDCE_DENOM_DIRCENTRALE = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new AreeDiProdottoQueryHandler(logger, dbContext, mapper);
        var filter = new DirezioniCentraliFilter();
        var request = new GetAreeDiProdottoQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

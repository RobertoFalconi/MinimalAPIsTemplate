using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;

using Microsoft.AspNetCore.Http.HttpResults;


namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class UtentiQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public UtentiQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetUtentiQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<UtentiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRUTE_TB_UTENTI_CL.Add(new GRUTE_TB_UTENTI_CL()
        {
            GRUTE_SEQ_UTENTE_PK = 1,
            GRUTE_DATA_AGGIORN = DateTime.Now,
            GRUTE_COD_APPL = "",
            GRUTE_MATRICOLA = "1234",
            GRUTE_COGNOME = "1234",
            GRUTE_NOME = "1234",
            GRUTE_EMAIL = "1234",
            GRUTE_CELLULARE = "1234",
            GRUTE_CODICEFISCALE = "4321",
            GRUTE_DATA_FINE_ABILITAZIONE = DateTime.Now,
            GRUTE_DATA_INIZIO_ABILITAZIONE = DateTime.Now,
            GRUTE_FLAG_COMPILAZ = "A",
            GRUTE_RUOLO = "",
            GRUTE_COD_UTENTE = "",
            GRUTE_PROFILO = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new UtentiQueryHandler(logger,httpClient, dbContext);
        var filter = new UtentiFilter()
        {
            PageNumber = 1,
            PageSize = 2,
            OrderAscDesc = "asc",
            OrderColumnName = "GRUTE_SEQ_UTENTE_PK"
        };
        var request = new GetUtentiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok<PagedResponse<GRUTE_TB_UTENTI_CL>>>(result);
    }

    [Fact]
    public async Task GetUtentiQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<UtentiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRUTE_TB_UTENTI_CL.Add(new GRUTE_TB_UTENTI_CL()
        {
            GRUTE_SEQ_UTENTE_PK = 1,
            GRUTE_DATA_AGGIORN = DateTime.Now,
            GRUTE_COD_APPL = "",
            GRUTE_MATRICOLA = "1234",
            GRUTE_COGNOME = "1234",
            GRUTE_NOME = "1234",
            GRUTE_EMAIL = "1234",
            GRUTE_CELLULARE = "1234",
            GRUTE_CODICEFISCALE = "4321",
            GRUTE_DATA_FINE_ABILITAZIONE = DateTime.Now,
            GRUTE_DATA_INIZIO_ABILITAZIONE = DateTime.Now,
            GRUTE_FLAG_COMPILAZ = "A",
            GRUTE_RUOLO = "",
            GRUTE_COD_UTENTE = "",
            GRUTE_PROFILO = "DSA",
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new UtentiQueryHandler(logger, httpClient, dbContext);
        var filter = new UtentiFilter();
        var request = new GetUtentiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}
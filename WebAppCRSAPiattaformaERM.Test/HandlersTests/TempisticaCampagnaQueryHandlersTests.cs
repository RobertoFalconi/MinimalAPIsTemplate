using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;

using Microsoft.AspNetCore.Http.HttpResults;


namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class TempisticaCampagnaQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public TempisticaCampagnaQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetTempisticaCampagnaQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaCampagnaQueryHandler(logger, dbContext);
        var filter = new TempisticaCampagnaFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "GRTCA_COD_UTENTE"
        };
        var request = new GetTempisticaCampagnaQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok<PagedResponse<GRTCA_TB_TEMPISTICACAMPAGNA_CL>>>(result);
    }

    [Fact]
    public async Task GetTempisticaCampagnaQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaCampagnaQueryHandler(logger, dbContext);
        var filter = new TempisticaCampagnaFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "ssss"
        };
        var request = new GetTempisticaCampagnaQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}

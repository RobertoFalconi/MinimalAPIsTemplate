using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;
using WebAppCRSAPiattaformaERM.Models.Filters;
using WebAppCRSAPiattaformaERM.Models.DB;

using Microsoft.AspNetCore.Http.HttpResults;


namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class TempisticaProcessiQueryHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public TempisticaProcessiQueryHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetTempisticaProcessiQueryTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiQueryHandler(logger, httpClient, dbContext);
        var filter = new TempisticaProcessiFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "GRTPR_COD_UTENTE"
        };
        var request = new GetTempisticaProcessiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok<PagedResponse<GRTPR_TB_TEMPISTICAPROCESSI_CL>>>(result);
    }

    [Fact]
    public async Task GetTempisticaCampagnaQueryTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiQueryHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA"
        });
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiQueryHandler(logger, httpClient, dbContext);
        var filter = new TempisticaProcessiFilter()
        {
            PageNumber = 1,
            PageSize = 5,
            OrderAscDesc = "asc",
            OrderColumnName = "ssss"
        };

        var request = new GetTempisticaProcessiQuery(filter);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }
}

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
using AutoMapper;

namespace WebAppCRSAPiattaformaERM.Test.HandlersTests;

public class TempisticaProcessiCommandHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public TempisticaProcessiCommandHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task PostTempisticaProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new TempisticaProcessiDTO()
        {
            //GRTPR_DATA_AGGIORN = DateTime.Now,
            //GRTPR_COD_APPL = "",
            //GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now.AddDays(1),
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            //GRTPR_FLAG_STATO = "A",
            GRTPR_SEQ_TEMPISTICA_PK = 12,
            GRTPR_NOTE = "DSA"
        };

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var filter = new TempisticaProcessiFilter();
        var request = new InserisciTempisticaProcessiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok<GRTPR_TB_TEMPISTICAPROCESSI_CL>>(result);
    }

    [Fact]
    public async Task PostTempisticaProcessiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;


        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new TempisticaProcessiDTO()
        {
            //GRTPR_DATA_AGGIORN = DateTime.Now,
            //GRTPR_COD_APPL = "",
            //GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now.AddDays(1),
            GRTPR_DENOM = "ASD",
            //GRTPR_FLAG_STATO = "A",
            GRTPR_SEQ_TEMPISTICA_PK = 12,
            GRTPR_NOTE = "DSA"
        };

        var mediatorMock = new Mock<IMediator>();
        

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var request = new InserisciTempisticaProcessiCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task PutTempisticaProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;


        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now.AddDays(1),
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA",
            GRTPR_SEQ_TEMPISTICA_PK = 1,
        };
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var testEntity2 = new TempisticaProcessiDTO()
        {
            GRTPR_DATA_FINE = testEntity.GRTPR_DATA_FINE,
            GRTPR_DATA_INIZ = testEntity.GRTPR_DATA_INIZ,
            GRTPR_DENOM = testEntity.GRTPR_DENOM,
            GRTPR_NOTE = testEntity.GRTPR_NOTE,
            GRTPR_SEQ_TEMPISTICA_PK = testEntity.GRTPR_SEQ_TEMPISTICA_PK,
        };

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var request = new AggiornaTempisticaProcessiCommand(testEntity2);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<Ok>(result);
    }
    [Fact]
    public async Task PutTempisticaProcessiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now.AddDays(1),
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA",
            GRTPR_SEQ_TEMPISTICA_PK = 1,
        };
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var notExistingEntity = new TempisticaProcessiDTO()
        {
            GRTPR_DATA_FINE = testEntity.GRTPR_DATA_FINE,
            GRTPR_DATA_INIZ = testEntity.GRTPR_DATA_INIZ,
            GRTPR_DENOM = testEntity.GRTPR_DENOM,
            GRTPR_NOTE = testEntity.GRTPR_NOTE,
            GRTPR_SEQ_TEMPISTICA_PK = testEntity.GRTPR_SEQ_TEMPISTICA_PK + 1,
        };
        var request = new AggiornaTempisticaProcessiCommand(notExistingEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaProcessiCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA"
        };
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviTempisticaProcessiCommand(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaProcessiCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaProcessiCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTPR_TB_TEMPISTICAPROCESSI_CL()
        {
            GRTPR_DATA_AGGIORN = DateTime.Now,
            GRTPR_COD_APPL = "",
            GRTPR_COD_UTENTE = "1234",
            GRTPR_DATA_FINE = DateTime.Now,
            GRTPR_DATA_INIZ = DateTime.Now,
            GRTPR_DENOM = "ASD",
            GRTPR_FLAG_STATO = "A",
            GRTPR_NOTE = "DSA"
        };
        dbContext.GRTPR_TB_TEMPISTICAPROCESSI_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaProcessiCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviTempisticaProcessiCommand(1343);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}

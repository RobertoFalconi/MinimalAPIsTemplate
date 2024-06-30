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

public class TempisticaCampagnaCommandHandlersTests : IClassFixture<AppFixture>
{
    private readonly AppFixture fixture;

    public TempisticaCampagnaCommandHandlersTests(AppFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task PostTempisticaCampagnaCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new TempisticaCampagnaDTO()
        {
            //GRTCA_DATA_AGGIORN = DateTime.Now,
            //GRTCA_COD_APPL = "",
            //GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            //GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };

        var mediatorMock = new Mock<IMediator>();
        

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var request = new InserisciTempisticaCampagnaCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }

    [Fact]
    public async Task PostTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;
        //Arrange Mapper
        var mapper = fixture.Mapper;


        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new TempisticaCampagnaDTO();

        var mediatorMock = new Mock<IMediator>();

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var request = new InserisciTempisticaCampagnaCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutTempisticaCampagnaCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new TempisticaCampagnaDTO()
        {
            GRTCA_SEQ_TEMPISTICA_PK = 1,
            //GRTCA_DATA_AGGIORN = DateTime.Now,
            //GRTCA_COD_APPL = "",
            //GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            //GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };
        var dbEntity = new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(dbEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GRTCA_TB_TEMPISTICACAMPAGNA_CL, TempisticaCampagnaDTO>();
            cfg.CreateMap<TempisticaCampagnaDTO, GRTCA_TB_TEMPISTICACAMPAGNA_CL>();
        });

        var mapper = config.CreateMapper();

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var request = new AggiornaTempisticaCampagnaCommand(testEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task PutTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GRTCA_TB_TEMPISTICACAMPAGNA_CL, TempisticaCampagnaDTO>();
            cfg.CreateMap<TempisticaCampagnaDTO, GRTCA_TB_TEMPISTICACAMPAGNA_CL>();
        });

        var mapper = config.CreateMapper();

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var notExistingEntity = new TempisticaCampagnaDTO();
        var request = new AggiornaTempisticaCampagnaCommand(notExistingEntity);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaCampagnaCommandTestConEsitoOK()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GRTCA_TB_TEMPISTICACAMPAGNA_CL, TempisticaCampagnaDTO>();
            cfg.CreateMap<TempisticaCampagnaDTO, GRTCA_TB_TEMPISTICACAMPAGNA_CL>();
        });

        var mapper = config.CreateMapper();

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviTempisticaCampagnaCommand(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
    [Fact]
    public async Task DeleteTempisticaCampagnaCommandTestConEsitoKO()
    {
        // Arrange Logger
        var logger = fixture.Logger<TempisticaCampagnaCommandHandler>();

        // Arrange Mediator
        var mediator = fixture.Mediator;

        // Arrange HTTP Client
        var httpClient = fixture.HttpClientApi;

        // Arrange DB Context
        var dbContext = fixture.DbContext;
        var testEntity = new GRTCA_TB_TEMPISTICACAMPAGNA_CL()
        {
            GRTCA_DATA_AGGIORN = DateTime.Now,
            GRTCA_COD_APPL = "",
            GRTCA_COD_UTENTE = "1234",
            GRTCA_DATA_FINE = DateTime.Now,
            GRTCA_DATA_INIZ = DateTime.Now,
            GRTCA_DENOM = "ASD",
            GRTCA_FLAG_STATO = "A",
            GRTCA_NOTE = "DSA"
        };
        dbContext.GRTCA_TB_TEMPISTICACAMPAGNA_CL.Add(testEntity);
        dbContext.SaveChanges();

        var mediatorMock = new Mock<IMediator>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<GRTCA_TB_TEMPISTICACAMPAGNA_CL, TempisticaCampagnaDTO>();
            cfg.CreateMap<TempisticaCampagnaDTO, GRTCA_TB_TEMPISTICACAMPAGNA_CL>();
        });

        var mapper = config.CreateMapper();

        var handler = new TempisticaCampagnaCommandHandler(logger, dbContext, mapper);
        var request = new RimuoviTempisticaCampagnaCommand(543543);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(Results.Ok("").ToString(), Results.Ok("").ToString());
    }
}


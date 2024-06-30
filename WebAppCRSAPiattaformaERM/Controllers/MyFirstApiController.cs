namespace MinimalSPAwithAPIs.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(AuthenticationSchemes = IdmAuthenticationOptions.DefaultScheme)]
public class MyFirstApiController : ControllerBase
{
    private readonly ILogger<MyFirstApiController> _logger;

    private readonly IMediator _mediator;

    public MyFirstApiController(ILogger<MyFirstApiController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public IResult Get([FromQuery] MyFirstApiFilter filter)
    {
        var mediator = _mediator.Send(new GetMyFirstApiQuery(filter));
        return mediator.Result;
    }

    [HttpPost]
    public IResult Post(MyFirstApiDTO model)
    {
        var mediator = _mediator.Send(new InserisciMyFirstApiCommand(model));
        return mediator.Result;
    }

    [HttpPut]
    public IResult Put(MyFirstApiDTO model)
    {
        var mediator = _mediator.Send(new AggiornaMyFirstApiCommand(model));
        return mediator.Result;
    }

    [HttpDelete]
    public IResult Delete(int id)
    {
        var mediator = _mediator.Send(new RimuoviMyFirstApiCommand(id));
        return mediator.Result;
    }
}

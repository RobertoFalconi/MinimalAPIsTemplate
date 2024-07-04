namespace MinimalSPAwithAPIs.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize(AuthenticationSchemes = IdmAuthenticationOptions.DefaultScheme)]
public class MyUsersController : ControllerBase
{
    private readonly ILogger<MyUsersController> _logger;

    private readonly IMediator _mediator;

    public MyUsersController(ILogger<MyUsersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public IResult Get([FromQuery] MyUsersFilter filter)
    {
        var mediator = _mediator.Send(new GetMyUsersQuery(filter));
        return mediator.Result;
    }

    [HttpGet("current")]
    public IResult GetCurrent()
    {
        var mediator = _mediator.Send(new GetCurrentUser(User));
        return mediator.Result;
    }

    [HttpPost]
    public IResult Post(MyUsersDTO model)
    {
        var mediator = _mediator.Send(new InserisciMyUsersCommand(model));
        return mediator.Result;
    }

    [HttpPut]
    public IResult Put(MyUsersDTO model)
    {
        var mediator = _mediator.Send(new AggiornaMyUsersCommand(model));
        return mediator.Result;
    }

    [HttpDelete]
    public IResult Delete(int id)
    {
        var mediator = _mediator.Send(new RimuoviMyUsersCommand(id));
        return mediator.Result;
    }
}

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

    [HttpPost]
    public async Task<IResult> Post(MyFirstApiDTO model)
    {
        var mediator = await _mediator.Send(new CreateMyFirstApiCommand(model));
        return mediator;
    }

    [HttpGet]
    public async Task<IResult> Get([FromQuery] MyFirstApiFilter filter)
    {
        var mediator = await _mediator.Send(new ReadMyFirstApiQuery(filter));
        return mediator;
    }

    [HttpPut]
    public async Task<IResult> Put(MyFirstApiDTO model)
    {
        var mediator = await _mediator.Send(new UpdateMyFirstApiCommand(model));
        return mediator;
    }

    [HttpDelete]
    public async Task<IResult> Delete(int id)
    {
        var mediator = await _mediator.Send(new DeleteMyFirstApiCommand(id));
        return mediator;
    }
}

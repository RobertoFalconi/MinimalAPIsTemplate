using GPTOpenAIwithDotNetAspireAPIs.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GPTOpenAIwithDotNetAspireAPIs.Controllers;

[ApiController]
[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = false)]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate-token")]
    public async Task<IActionResult> GenerateToken([FromBody] string userId)
    {
        var token = await _mediator.Send(new GenerateTokenCommand(userId));
        return Ok(new { token });
    }
}
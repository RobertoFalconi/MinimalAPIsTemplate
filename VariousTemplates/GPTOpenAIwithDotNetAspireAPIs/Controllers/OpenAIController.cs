using GPTOpenAIwithDotNetAspireAPIs.Commands;
using GPTOpenAIwithDotNetAspireAPIs.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPTOpenAIwithDotNetAspireAPIs.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OpenAIController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpenAIController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("completion")]
    [ProducesResponseType<OpenAIResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetCompletion([FromBody] OpenAIRequestModel request)
    {
        if (string.IsNullOrEmpty(request.UserMessage))
            return BadRequest("UserMessage required.");

        var command = new OpenAICompletionCommand(
            request.UserMessage,
            request.SystemPrompt,
            request.Model,
            request.Temperature,
            request.MaxTokens,
            request.CacheMinutes);

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ex.Message);
        }
    }

    [HttpPost("simple")]
    [ProducesResponseType<OpenAIResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSimpleCompletion([FromBody] string prompt, [FromQuery] string? model = null)
    {
        if (string.IsNullOrEmpty(prompt))
            return BadRequest("Prompt required.");

        var command = new OpenAICompletionCommand(prompt, Model: model);

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ex.Message);
        }
    }

    [HttpPost("file")]
    [ProducesResponseType<OpenAIResponseModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessFile(IFormFile file, [FromForm] string? systemPrompt = null, [FromForm] string? model = null)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not valid.");

        string fileContent;
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            fileContent = await reader.ReadToEndAsync();
        }

        var command = new OpenAIFileProcessingCommand(fileContent, systemPrompt, model);

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(502, ex.Message);
        }
    }
}
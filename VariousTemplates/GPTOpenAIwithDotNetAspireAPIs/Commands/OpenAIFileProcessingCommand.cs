using GPTOpenAIwithDotNetAspireAPIs.Models;
using MediatR;

namespace GPTOpenAIwithDotNetAspireAPIs.Commands;

public record OpenAIFileProcessingCommand(
    string FileContent,
    string? SystemPrompt = null,
    string? Model = "gpt-4"
) : IRequest<OpenAIResponseModel>;
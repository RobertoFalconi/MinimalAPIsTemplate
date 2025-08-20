using GPTOpenAIwithDotNetAspireAPIs.Models;
using MediatR;

namespace GPTOpenAIwithDotNetAspireAPIs.Commands;

public record OpenAICompletionCommand(
    string UserMessage,
    string? SystemPrompt = null,
    string? Model = "gpt-4",
    double? Temperature = 0.7,
    int? MaxTokens = null,
    int? CacheMinutes = 5
) : IRequest<OpenAIResponseModel>;
using MediatR;

namespace GPTOpenAIwithDotNetAspireAPIs.Commands;

public record GenerateTokenCommand(string UserId) : IRequest<string>;
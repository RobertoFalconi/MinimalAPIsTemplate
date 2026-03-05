using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Commands;

public class GenerateTokenCommandHandler : ICommandHandler<GenerateTokenCommand, TokenResponse>
{
    // Demo credentials — replace with a real user store in production
    private const string DemoUsername = "string";
    private const string DemoPassword = "string";

    private readonly IJwtTokenService _jwtTokenService;

    public GenerateTokenCommandHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public Task<TokenResponse> HandleAsync(GenerateTokenCommand command)
    {
        if (command.Username != DemoUsername || command.Password != DemoPassword)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _jwtTokenService.GenerateToken(command.Username);
        return Task.FromResult(token);
    }
}

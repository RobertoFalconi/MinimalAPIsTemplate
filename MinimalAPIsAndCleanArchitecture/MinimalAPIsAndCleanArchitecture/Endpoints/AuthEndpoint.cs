using MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;
using MinimalAPIsAndCleanArchitecture.Core.Application.Commands;
using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;

namespace MinimalAPIsAndCleanArchitecture.Endpoints;

internal static class AuthEndpoint
{
    public static void MapAuth(this WebApplication app)
    {
        app.MapPost("/auth/token",
            async (GenerateTokenCommand command,
                   ICommandHandler<GenerateTokenCommand, TokenResponse> handler) =>
            {
                try
                {
                    var token = await handler.HandleAsync(command);
                    return Results.Ok(token);
                }
                catch (UnauthorizedAccessException)
                {
                    return Results.Unauthorized();
                }
            })
            .WithName("GenerateToken")
            .AllowAnonymous();
    }
}

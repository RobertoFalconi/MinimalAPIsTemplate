namespace MinimalAPIs.Handlers.ServiceHandlers;

public sealed record GenerateTokenRequest() : IRequest<string>;

public sealed class TokenHandler : IRequestHandler<GenerateTokenRequest, string>
{
    public async Task<string> Handle(GenerateTokenRequest request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken(new JwtHeader(), new JwtPayload());
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
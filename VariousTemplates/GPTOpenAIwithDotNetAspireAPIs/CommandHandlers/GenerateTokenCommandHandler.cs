using GPTOpenAIwithDotNetAspireAPIs.Commands;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPTOpenAIwithDotNetAspireAPIs.Handlers;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, string>
{
    private readonly byte[] _key;

    public GenerateTokenCommandHandler(IConfiguration config)
    {
        var secretKey = config["Jwt:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("JWT secret key not configured or empty.");

        if (secretKey.Length < 32)
            throw new InvalidOperationException("JWT secret key must be at least 32 characters long for security.");

        _key = Encoding.ASCII.GetBytes(secretKey);
    }

    public Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMonths(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}
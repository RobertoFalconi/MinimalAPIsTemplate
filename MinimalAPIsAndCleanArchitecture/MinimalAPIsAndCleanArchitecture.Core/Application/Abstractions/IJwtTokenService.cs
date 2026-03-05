using MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;

namespace MinimalAPIsAndCleanArchitecture.Core.Application.Abstractions;

public interface IJwtTokenService
{
    TokenResponse GenerateToken(string username);
}

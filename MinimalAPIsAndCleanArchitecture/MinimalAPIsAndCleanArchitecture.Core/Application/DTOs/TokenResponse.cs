namespace MinimalAPIsAndCleanArchitecture.Core.Application.DTOs;

public record TokenResponse(string Token, DateTime ExpiresAt);

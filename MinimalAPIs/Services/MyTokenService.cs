namespace MinimalAPIs.Services;

public class MyTokenService
{
    public async Task<string> GenerateToken()
    {
        var jwtHeader = new JwtHeader();

        var jwtPayload = new JwtPayload();
        jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

        var token = new JwtSecurityToken(jwtHeader, jwtPayload);

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateSignedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateSignedTokenFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateEncryptedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateEncryptedTokenFromCertificate(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}

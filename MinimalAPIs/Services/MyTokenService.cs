using System.Security.Claims;

namespace MinimalAPIs.Services;

public class MyTokenService
{
    public async Task<string> GenerateToken()
    {
        var token = new JwtSecurityToken(new JwtHeader(), new JwtPayload());
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateSignedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateSignedTokenFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateEncryptedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateEncryptedTokenFromCertificate(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), DateTime.Now, new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512), new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    // TODO: Il parametro "alg" è uguale a "ES256"; Il parametro "enc" è uguale a "AES-GCM"
    public async Task<string> GenerateJOSE()
    {
        var header = new JwtHeader(new SigningCredentials(new ECDsaSecurityKey(ECDsa.Create()), SecurityAlgorithms.EcdsaSha256));
        var payload = new JwtPayload(issuer: "Issuer", audience: "Audience", claims: new List<Claim> { new Claim(ClaimTypes.Name, "UserName") }, notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(30));
        var encryptedJwt = new JwtSecurityToken(header, payload);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret key"));

        // Define the encryption algorithm and key
        var encryptionAlgorithm = SecurityAlgorithms.Aes256Gcm;
        var encryptingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret key"));

        // Encrypt the JWT
        var encryptedToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer: "Issuer", audience: "Audience", new ClaimsIdentity(), notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(30), issuedAt: DateTime.Now, new SigningCredentials(new ECDsaSecurityKey(ECDsa.Create()), SecurityAlgorithms.EcdsaSha256));

        // Serialize the token to a string
        var token = new JwtSecurityTokenHandler().WriteToken(encryptedToken);

        return token;
    }
}

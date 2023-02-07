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

    public async Task<string> GenerateJOSEFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey, X509SecurityKey encryptingCertificateKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: issuer, audience: audience, subject: new ClaimsIdentity(), notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(30), issuedAt: DateTime.Now,
            signingCredentials: new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSsaPssSha512),
            encryptingCredentials: new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateEncryptedTokenNotSigned(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
    {
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer: issuer, audience: audience, subject: null, notBefore: null, expires: null, issuedAt: null, signingCredentials: null, new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512));
        token.Payload.AddClaim(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<string> GenerateJOSERandomlySigned()
    {
        var rsa = RSA.Create();
        var privateKey = rsa.ExportParameters(true);
        var publicKey = rsa.ExportParameters(false);

        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
            issuer: "Issuer", audience: "Audience", subject: new ClaimsIdentity(), notBefore: DateTime.Now, expires: DateTime.Now.AddMinutes(30), issuedAt: DateTime.Now,
            signingCredentials: new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256),
            encryptingCredentials: new EncryptingCredentials(new RsaSecurityKey(publicKey), SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
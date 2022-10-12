using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MinimalAPIs.Services
{
    public class MyTokenService
    {
        public async Task<string> GenerateToken()
        {
            var jwtHeader = new JwtHeader();
            jwtHeader.Add("kid", "YourKid");

            var jwtPayload = new JwtPayload();
            jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload)));
        }

        public async Task<string> GenerateSignedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
        {
            var jwtHeader = new JwtHeader(new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature));
            jwtHeader.Add("kid", "YourKid");

            var jwtPayload = new JwtPayload(issuer, audience, null, null, DateTime.Now.AddMinutes(30), null);
            jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload)));
        }

        public async Task<string> GenerateSignedTokenFromCertificate(string issuer, string audience, X509SecurityKey asymmetricKey)
        {
            var jwtHeader = new JwtHeader(new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512));

            var jwtPayload = new JwtPayload(issuer, audience, null, null, DateTime.Now.Add(new TimeSpan(0, 0, 1800)), null);
            jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload)));
        }

        public async Task<string> GenerateEncryptedToken(string issuer, string audience, SymmetricSecurityKey symmetricKey)
        {
            var ep = new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), null, new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature), ep);
            token.Header.Add("kid", "YourKid");
            token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<string> GenerateEncryptedTokenFromCertificate(string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey asymmetricKey)
        {
            var ep = new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), null, new SigningCredentials(asymmetricKey, SecurityAlgorithms.RsaSha512), ep);
            token.Header.Add("kid", "YourKid");
            token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "YourCustomClaim"));

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}

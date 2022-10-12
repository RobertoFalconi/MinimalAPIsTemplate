using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MinimalAPIs.Services
{
    public class MyTokenService
    {
        public async Task<string> GenerateToken(string issuer, string audience, SymmetricSecurityKey key)
        {
            var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature));
            jwtHeader.Add("kid", "PartitaIVA");
            
            var jwtPayload = new JwtPayload(issuer, audience, null, null, DateTime.Now.AddMinutes(30), null);
            jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
            
            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload)));
        }
    }
}

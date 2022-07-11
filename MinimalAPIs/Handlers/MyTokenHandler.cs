using Microsoft.IdentityModel.Tokens;
using MinimalAPIs.Services;
using System.IdentityModel.Tokens.Jwt;

namespace MinimalAPIs.Handlers
{
    public class MyTokenHandler //: Microsoft.IdentityModel.Tokens.TokenHandler
    {
        
        private readonly IMinimalService _tokenService;

        public MyTokenHandler(IMinimalService tokenService)
        {
            _tokenService = tokenService;
        }

        public void RegisterTokenAPIs(WebApplication app, SymmetricSecurityKey key)
        {
            app.MapGet("/generateToken", () =>
            {
                var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature));
                var jwtPayload = new JwtPayload("YourIssuer", "YourAudience", null, null, DateTime.Now.AddMinutes(30), null);
                jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
                return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload));
            });

            app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

            app.MapGet("/generateTokenEncrypted", () =>
            {
                var ep = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);
                var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken("YourIssuer", "YourAudience", null, null, DateTime.Now.AddHours(1), null, new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature), ep);
                token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
                return new JwtSecurityTokenHandler().WriteToken(token);
            });

            app.MapGet("/getDouble", (IMinimalService minimalService, int a) => minimalService.Double(a, new CancellationToken()));
        }
    }

}

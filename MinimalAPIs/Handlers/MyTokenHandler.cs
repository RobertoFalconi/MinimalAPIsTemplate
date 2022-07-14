using Microsoft.IdentityModel.Tokens;
using MinimalAPIs.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MinimalAPIs.Handlers
{
    public class MyTokenHandler //: Microsoft.IdentityModel.Tokens.TokenHandler
    {

        private readonly IMyTokenService _tokenService;

        public MyTokenHandler(IMyTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public void RegisterAPIs(WebApplication app)
        {
            var issuer = app.Configuration["Jwt:Issuer"];
            var audience = app.Configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.Configuration["Jwt:Key"]));
            var keyCert = new X509SecurityKey(new X509Certificate2(app.Configuration["Certificate:Path"], app.Configuration["Certificate:Password"]));

            app.MapGet("/generateToken", () =>
            {
                var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature));
                jwtHeader.Add("kid","PartitaIVA");
                var jwtPayload = new JwtPayload(issuer, audience, null, null, DateTime.Now.AddMinutes(30), null);
                jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
                return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload));
            });

            app.MapGet("/generateTokenEncrypted", () =>
            {
                var ep = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);
                var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(issuer, audience, null, null, DateTime.Now.AddHours(1), null, new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature), ep);
                token.Header.Add("kid","PartitaIVA");
                token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
                return new JwtSecurityTokenHandler().WriteToken(token);
            });

            app.MapGet("/generateTokenFromCertificate", () =>
            {
                var jwtHeader = new JwtHeader(new SigningCredentials(keyCert, SecurityAlgorithms.RsaSha256));
                var jwtPayload = new JwtPayload(issuer, audience, null, null, DateTime.Now.Add(new TimeSpan(0, 0, 1800)), null);
                jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
                return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload));
            });

            app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

            app.MapGet("/getDouble", (int a) => _tokenService.Double(a, new CancellationToken()));
        }
    }
}

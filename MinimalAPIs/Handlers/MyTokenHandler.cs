namespace MinimalAPIs.Handlers;

public class MyTokenHandler
{
    public void RegisterAPIs(WebApplication app)
    {
        var logger = app.Logger;

        logger.LogInformation("Inizio recupero parametri");
        
        var issuer = app.Configuration["Jwt:Issuer"]!;
        var audience = app.Configuration["Jwt:Audience"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.Configuration["Jwt:Key"]!));
        var keyCert = new X509SecurityKey(new X509Certificate2(app.Configuration["Certificate:Path"]!, app.Configuration["Certificate:Password"]));

        app.MapGet("/generateToken", async () =>
        {
            var token = await new MyTokenService().GenerateToken();
            return token;
        });

        app.MapGet("/generateSignedToken", async () =>
        {
            var token = await new MyTokenService().GenerateSignedToken(issuer, audience, key);
            return token;
        });

        app.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateSignedTokenFromCertificate(issuer, audience, keyCert);
            return token;
        });

        app.MapGet("/generateEncryptedToken", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedToken(issuer, audience, key);
            return token;
        });

        app.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenFromCertificate(issuer, audience, key, keyCert);
            return token;
        });

        app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        app.MapGet("/recurringTryToken", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();
            var service = new MyTokenService();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();
        
        app.MapGet("/RemoveRecurringJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();
    }
}

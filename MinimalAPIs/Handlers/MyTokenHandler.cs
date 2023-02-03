namespace MinimalAPIs.Handlers;

public class MyTokenHandler
{
    public void RegisterAPIs(WebApplication app, string issuer, string audience, SymmetricSecurityKey key, X509SecurityKey keyCert)
    {
        var logger = app.Logger;

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

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();
        
        app.MapGet("/RemoveRecurringJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        app.MapGet("/tryNLog", () =>
        {
            logger.LogCritical("This is a critical good sample");
            return Results.Ok();
        });
    }
}

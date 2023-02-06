namespace MinimalAPIs.Handlers;

public class MyEndpointHandler
{
    public void RegisterAPIs(WebApplication app, string issuer, string audience, SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var logger = app.Logger;

        var tokenHandler = app.MapGroup("/token").WithTags("Token Service API");

        var hangfireHandler = app.MapGroup("/hangfire").WithTags("Hangfire Service API");

        var nlogHandler = app.MapGroup("/nlog").WithTags("NLog Service API");

        var compressingHandler = app.MapGroup("/compressing").WithTags("Compressing Service API");

        var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();

        _ = tokenHandler.MapGet("/generateToken", async () =>
        {
            var token = await new MyTokenService().GenerateToken();
            return token;
        });

        _ = tokenHandler.MapGet("/generateSignedToken", async () =>
        {
            var token = await new MyTokenService().GenerateSignedToken(issuer, audience, key);
            return token;
        });

        _ = tokenHandler.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateSignedTokenFromCertificate(issuer, audience, signingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateEncryptedToken", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedToken(issuer, audience, key);
            return token;
        });

        _ = tokenHandler.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenFromCertificate(issuer, audience, key, signingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateJOSEFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateJOSEFromCertificate(issuer, audience, signingCertificateKey, encryptingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateEncryptedTokenNotSigned", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenNotSigned(issuer, audience, key, signingCertificateKey);
            return token;
        });

        _ = tokenHandler.MapGet("/generateJOSERandomlySigned", async () =>
        {
            var token = await new MyTokenService().GenerateJOSERandomlySigned();
            return token;
        });

        _ = tokenHandler.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        _ = hangfireHandler.MapGet("/recurringTryToken", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        _ = hangfireHandler.MapGet("/removeRecurringJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        _ = hangfireHandler.MapGet("/recurringSampleJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        });

        _ = hangfireHandler.MapGet("/removeSampleJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("SampleJob");
        });

        _ = nlogHandler.MapGet("/tryNLog", () =>
        {
            logger.LogCritical("This is a critical good sample");
            return Results.Ok();
        });

        _ = compressingHandler.MapGet("/tryCompression", async () =>
        {
            var jsonToCompress = JsonSerializer.Serialize(forecast);

            var compressedData = await new MyCompressingService().Compress(jsonToCompress);

            return compressedData;
        });

        _ = compressingHandler.MapGet("/tryDecompression", async (string compressedData) =>
        {
            var decompressedData = await new MyCompressingService().Decompress(compressedData);

            return decompressedData;
        });

        _ = nlogHandler.MapGet("/getLogsWithLINQ", async () =>
        {
            List<Nlog> logs;
            using (var context = new MinimalApisDbContext())
            {
                using var dbContextTransaction = await context.Database.BeginTransactionAsync();
                logs = await (from l in context.Nlog
                              where true
                              select l).ToListAsync();
            }

            return logs;
        });
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
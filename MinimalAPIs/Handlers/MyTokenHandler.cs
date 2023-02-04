using System.IO.Compression;
using System.Text;

namespace MinimalAPIs.Handlers;

public class MyTokenHandler
{
    public void RegisterAPIs(WebApplication app, string issuer, string audience, SymmetricSecurityKey key, X509SecurityKey keyCert)
    {
        var logger = app.Logger;

        _ = app.MapGet("/generateToken", async () =>
        {
            var token = await new MyTokenService().GenerateToken();
            return token;
        });

        _ = app.MapGet("/generateSignedToken", async () =>
        {
            var token = await new MyTokenService().GenerateSignedToken(issuer, audience, key);
            return token;
        });

        _ = app.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateSignedTokenFromCertificate(issuer, audience, keyCert);
            return token;
        });

        _ = app.MapGet("/generateEncryptedToken", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedToken(issuer, audience, key);
            return token;
        });

        _ = app.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenFromCertificate(issuer, audience, key, keyCert);
            return token;
        });

        _ = app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        _ = app.MapGet("/recurringTryToken", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        _ = app.MapGet("/RemoveRecurringJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        _ = app.MapGet("/tryNLog", () =>
        {
            logger.LogCritical("This is a critical good sample");
            return Results.Ok();
        });

        _ = app.MapGet("/tryCompression", async () =>
        {
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

            var jsonToCompress = JsonSerializer.Serialize(forecast);

            var compressedData = await new MyCompressingService().Compress(jsonToCompress);

            return compressedData;
        });

        _ = app.MapGet("/tryDecompression", async (string compressedData) =>
        {
            var decompressedData = await new MyCompressingService().Decompress(compressedData);

            return decompressedData;
        });
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
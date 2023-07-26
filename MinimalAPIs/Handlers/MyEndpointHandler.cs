namespace MinimalAPIs.Handlers;

public class MyEndpointHandler
{
    public void RegisterAPIs(WebApplication app, string issuer, string audience, SymmetricSecurityKey key, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var logger = app.Logger;

        var tokenHandler = app.MapGroup("/token").WithTags("Token Service API");

        var hangfireHandler = app.MapGroup("/hangfire").WithTags("Hangfire Service API");

        var nlogHandler = app.MapGroup("/nlog").WithTags("NLog, EF and Dapper Services API");

        var compressingHandler = app.MapGroup("/compressing").WithTags("Compressing Service API");

        var httpClientHandler = app.MapGroup("/httpClient").WithTags("HTTP client to call another REST API");

        var mediatr = app.MapGroup("/mediatr").WithTags("MediatR APIs");

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

        tokenHandler.MapGet("/generateToken", async () =>
        {
            var token = await new MyTokenService().GenerateToken();
            return token;
        });

        tokenHandler.MapGet("/generateSignedToken", async () =>
        {
            var token = await new MyTokenService().GenerateSignedToken(issuer, audience, key);
            return token;
        });

        tokenHandler.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateSignedTokenFromCertificate(issuer, audience, signingCertificateKey);
            return token;
        });

        tokenHandler.MapGet("/generateEncryptedToken", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedToken(issuer, audience, key);
            return token;
        });

        tokenHandler.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenFromCertificate(issuer, audience, key, signingCertificateKey);
            return token;
        });

        tokenHandler.MapGet("/generateJOSEFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateJOSEFromCertificate(issuer, audience, signingCertificateKey, encryptingCertificateKey);
            return token;
        });

        tokenHandler.MapGet("/generateEncryptedTokenNotSigned", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenNotSigned(issuer, audience, key, signingCertificateKey);
            return token;
        });

        tokenHandler.MapGet("/generateJOSERandomlySigned", async () =>
        {
            var token = await new MyTokenService().GenerateJOSERandomlySigned();
            return token;
        });

        tokenHandler.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        hangfireHandler.MapGet("/recurringTryToken", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        hangfireHandler.MapGet("/removeRecurringJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        hangfireHandler.MapGet("/recurringSampleJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        });

        hangfireHandler.MapGet("/removeSampleJob", () =>
        {
            logger.LogInformation("Inizio RecurringJob");

            var manager = new RecurringJobManager();

            manager.RemoveIfExists("SampleJob");
        });

        nlogHandler.MapGet("/tryNLog", () =>
        {
            logger.LogCritical("This is a critical good sample");
            return Results.Ok();
        });

        compressingHandler.MapGet("/tryCompression", async () =>
        {
            var jsonToCompress = JsonSerializer.Serialize(forecast);

            var compressedData = await new MyCompressingService().Compress(jsonToCompress);

            return compressedData;
        });

        compressingHandler.MapGet("/tryDecompression", async (string compressedData) =>
        {
            var decompressedData = await new MyCompressingService().Decompress(compressedData);
            return decompressedData;
        });

        nlogHandler.MapGet("/getLogsWithEntityFrameworkAndLinq", async (int page, int pageSize, IDbContextFactory<MinimalApisDbContext> dbContextFactory) =>
        {
            var stopwatch = Stopwatch.StartNew();
            List<Nlog> logs;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1;
            using (var context = await dbContextFactory.CreateDbContextAsync())
            {
                using var dbContextTransaction = await context.Database.BeginTransactionAsync();
                logs = await (from l in context.Nlog
                              select l).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            return Results.Ok(new { elapsedTime, logs });
        });

        nlogHandler.MapGet("/getLogsWithDapperAndSql", async () =>
        {
            var stopwatch = Stopwatch.StartNew();

            using var connection = new SqlConnection(app.Configuration.GetConnectionString("MinimalAPIsDB"));
            var logs = (await connection.QueryAsync<Nlog>("SELECT * FROM NLog WHERE 1 = @param ",
                new { param = (int?)1 })).ToList();

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            return Results.Ok(new { elapsedTime, logs });
        });

        httpClientHandler.MapGet("/getAnotherEndpoint", async (HttpContext context) =>
        {
            var request = context?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}{request?.QueryString}";
            baseUrl = baseUrl.Last().ToString() != "/" ? baseUrl.TrimEnd('/') : baseUrl;

            var client = new HttpClient();
            using var response = await client.GetAsync(new Uri(baseUrl + "/httpClient/getThisEndpoint/"), HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return response.Content.Headers.ContentType?.MediaType == System.Net.Mime.MediaTypeNames.Application.Json
                ? await response.Content.ReadFromJsonAsync<dynamic>()
                : await response.Content.ReadAsStringAsync();
        });

        httpClientHandler.MapGet("/getAnotherEndpointOptimized", async (HttpContext context) =>
        {
            var request = context.Request;
            var baseUrl = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, null, request.QueryString);

            var client = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
            using var response = await client.GetAsync(baseUrl + "httpClient/getThisEndpoint/", HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.Headers.ContentType?.MediaType == System.Net.Mime.MediaTypeNames.Application.Json
                    ? await response.Content.ReadFromJsonAsync<dynamic>()
                    : await response.Content.ReadAsStringAsync();
            }
            else
            {
                var problem = new ProblemDetails
                {
                    Status = (int)response.StatusCode,
                    Title = response.ReasonPhrase,
                    Detail = await response.Content.ReadAsStringAsync()
                };
                return Results.Problem(problem);
            }
        });


        httpClientHandler.MapGet("/getThisEndpoint", () => Results.Ok(new { res = "This is the response from httpClient/getThisEndpoint !" }));

        mediatr.MapGet("/getNotified", async (IMediator mediator) =>
        {
            await mediator.Publish(new Notification("Hello!"));
            return Results.Ok("Notified");
        });

        mediatr.MapGet("/sendRequest", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new MyRequest("How are you?"));
            return Results.Ok(response);
        });
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF
    {
        get
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    }
}
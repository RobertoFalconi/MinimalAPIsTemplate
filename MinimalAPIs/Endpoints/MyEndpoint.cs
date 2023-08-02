namespace MinimalAPIs.Endpoints;

public static class MyEndpoint
{
    public static void MapMyEndpoint(this WebApplication app, string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var logger = app.Logger;

        var token = app.MapGroup("/token").WithTags("Token APIs");

        var hangfire = app.MapGroup("/hangfire").WithTags("Hangfire APIs");

        var nlog = app.MapGroup("/nlog").WithTags("NLog, EF and Dapper APIs");

        var entityFramework = app.MapGroup("/nlog").WithTags("Entity Framework Core APIs");

        var dapper = app.MapGroup("/nlog").WithTags("Dapper APIs");

        var compressing = app.MapGroup("/compressing").WithTags("Compressing APIs");

        var httpClient = app.MapGroup("/httpClient").WithTags("HTTP Client APIs");

        var mediatr = app.MapGroup("/mediatr").WithTags("MediatR APIs");

        var fluentValidation = app.MapGroup("/fluentValidation").WithTags("Validation APIs");

        var benchmarks = app.MapGroup("/benchmarks").WithTags("Benchmarks");

        token.MapGet("/generateToken", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateToken();

            return token;
        });

        token.MapGet("/generateSignedToken", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateSignedToken(issuer, audience, symmetricKey);

            return token;
        });

        token.MapGet("/generateSignedTokenFromCertificate", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateSignedTokenFromCertificate(issuer, audience, signingCertificateKey);

            return token;
        });

        token.MapGet("/generateEncryptedToken", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateEncryptedToken(issuer, audience, symmetricKey);

            return token;
        });

        token.MapGet("/generateEncryptedTokenFromCertificate", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateEncryptedTokenFromCertificate(issuer, audience, symmetricKey, signingCertificateKey);

            return token;
        });

        token.MapGet("/generateJOSEFromCertificate", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateJOSEFromCertificate(issuer, audience, signingCertificateKey, encryptingCertificateKey);

            return token;
        });

        token.MapGet("/generateEncryptedTokenNotSigned", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateEncryptedTokenNotSigned(issuer, audience, symmetricKey, signingCertificateKey);

            return token;
        });

        token.MapGet("/generateJOSERandomlySigned", async (MyTokenService tokenService) =>
        {
            var token = await tokenService.GenerateJOSERandomlySigned();

            return token;
        });

        token.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        token.MapGet("/tryTokenPlusCustomPolicy", () => Results.Ok()).RequireAuthorization("IsAuthorized");

        hangfire.MapGet("/recurringTryToken", (IRecurringJobManager manager) =>
        {
            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        hangfire.MapGet("/removeRecurringJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        hangfire.MapGet("/recurringSampleJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        });

        hangfire.MapGet("/removeSampleJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.RemoveIfExists("SampleJob");
        });

        compressing.MapGet("/tryCompression", async (MyCompressingService compressingService) =>
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastAPI
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                )).ToArray();

            var jsonToCompress = JsonSerializer.Serialize(forecast);

            var compressedData = await compressingService.Compress(jsonToCompress);

            return compressedData;
        });

        compressing.MapGet("/tryDecompression", async (MyCompressingService compressingService, string compressedData) =>
        {
            var decompressedData = await compressingService.Decompress(compressedData);

            return decompressedData;
        });

        nlog.MapGet("/tryNLog", () =>
        {
            logger.LogError("This is a critical good sample");

            return Results.Ok();
        });

        entityFramework.MapGet("/logsWithEntityFrameworkAndLinq", async (int page, int pageSize, IDbContextFactory<MinimalApisDbContext> dbContextFactory) =>
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

        dapper.MapGet("/logsWithDapperAndSql", async () =>
        {
            var stopwatch = Stopwatch.StartNew();

            using var connection = new SqlConnection(app.Configuration.GetConnectionString("MinimalAPIsDB"));
            var logs = (await connection.QueryAsync<Nlog>("SELECT * FROM NLog WHERE 1 = @param ",
                new { param = (int?)1 })).ToList();

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            return Results.Ok(new { elapsedTime, logs });
        });

        httpClient.MapGet("/callAnotherEndpoint", async (HttpContext context) =>
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

        httpClient.MapGet("/callAnotherEndpointOptimized", async (HttpContext context) =>
        {
            var request = context.Request;
            var baseUrl = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, null, request.QueryString);

            var client = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
            using var response = await client.GetAsync(baseUrl + "httpClient/callThisEndpoint/", HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.Headers.ContentType?.MediaType == System.Net.Mime.MediaTypeNames.Application.Json
                    ? await response.Content.ReadFromJsonAsync<dynamic>()
                    : await response.Content.ReadAsStringAsync();
            }
            else
            {
                var problemDetail = await response.Content.ReadAsStringAsync();
                return Results.Problem(detail: problemDetail, statusCode: (int)response.StatusCode, title: response.ReasonPhrase);
            }
        });

        httpClient.MapGet("/callThisEndpoint", () => Results.Ok(new { res = "This is the response from httpClient/callThisEndpoint!" }));

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

        fluentValidation.MapPost("/tryValidation", async ([FromBody] CustomerAPI customer) =>
        {
            var validationResult = new CustomerValidator().Validate(customer);

            if (validationResult.IsValid)
            {
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest(validationResult.Errors);
            }
        });

        app.MapPost("/recurringRequest", (IMediator mediator) =>
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Bridge()), Cron.Minutely());

            return Results.Accepted();
        });

        benchmarks.MapGet("/evaluateServiceInitialization", async () =>
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 100000; i++)
            {

                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecastAPI
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    )).ToArray();

                var jsonToCompress = JsonSerializer.Serialize(forecast);

                var compressingService = new MyCompressingService();

                var response = await compressingService.Compress(jsonToCompress);
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            return Results.Ok(new { elapsedTime });
        });

        benchmarks.MapGet("/evaluateDependencyInjection", async (MyCompressingService compressingService) =>
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 100000; i++)
            {

                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecastAPI
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    )).ToArray();

                var jsonToCompress = JsonSerializer.Serialize(forecast);

                var response = await compressingService.Compress(jsonToCompress);
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            return Results.Ok(new { elapsedTime });
        });

        benchmarks.MapPost("/evaluateMediatR", async (IMediator mediator) =>
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 100000; i++)
            {

                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecastAPI
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    )).ToArray();

                var jsonToCompress = JsonSerializer.Serialize(forecast);

                var response = await mediator.Send(new CompressRequest(jsonToCompress));
            }

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            return Results.Ok(new { elapsedTime });
        });
    }

    public static async Task<int> Bridge()
    {
        return 1;
    }
}
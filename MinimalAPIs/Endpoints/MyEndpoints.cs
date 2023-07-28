namespace MinimalAPIs.Endpoints;

public static class MyEndpoints
{
    public static void MapMyEndpoints(this WebApplication app, string issuer, string audience, SymmetricSecurityKey symmetricKey, X509SecurityKey signingCertificateKey, X509SecurityKey encryptingCertificateKey)
    {
        var logger = app.Logger;

        var tokenRouteBuilder = app.MapGroup("/token").WithTags("Token APIs");

        var hangfireRouteBuilder = app.MapGroup("/hangfire").WithTags("Hangfire APIs");

        var nlogRouteBuilder = app.MapGroup("/nlog").WithTags("NLog, EF and Dapper APIs");

        var compressingRouteBuilder = app.MapGroup("/compressing").WithTags("Compressing APIs");

        var httpClientRouteBuilder = app.MapGroup("/httpClient").WithTags("HTTP Client APIs");

        var mediatrRouteBuilder = app.MapGroup("/mediatr").WithTags("MediatR APIs");

        var fluentValidation = app.MapGroup("/fluentValidation").WithTags("Validation APIs");

        tokenRouteBuilder.MapGet("/generateToken", async () =>
        {
            var token = await new MyTokenService().GenerateToken();

            return token;
        });

        tokenRouteBuilder.MapGet("/generateSignedToken", async () =>
        {
            var token = await new MyTokenService().GenerateSignedToken(issuer, audience, symmetricKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateSignedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateSignedTokenFromCertificate(issuer, audience, signingCertificateKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateEncryptedToken", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedToken(issuer, audience, symmetricKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateEncryptedTokenFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenFromCertificate(issuer, audience, symmetricKey, signingCertificateKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateJOSEFromCertificate", async () =>
        {
            var token = await new MyTokenService().GenerateJOSEFromCertificate(issuer, audience, signingCertificateKey, encryptingCertificateKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateEncryptedTokenNotSigned", async () =>
        {
            var token = await new MyTokenService().GenerateEncryptedTokenNotSigned(issuer, audience, symmetricKey, signingCertificateKey);

            return token;
        });

        tokenRouteBuilder.MapGet("/generateJOSERandomlySigned", async () =>
        {
            var token = await new MyTokenService().GenerateJOSERandomlySigned();

            return token;
        });

        tokenRouteBuilder.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

        hangfireRouteBuilder.MapGet("/recurringTryToken", () =>
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate("RecurringJobId", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        }).RequireAuthorization();

        hangfireRouteBuilder.MapGet("/removeRecurringJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.RemoveIfExists("RecurringJobId");
        }).RequireAuthorization();

        hangfireRouteBuilder.MapGet("/recurringSampleJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate("SampleJob", Job.FromExpression(() => Results.Ok(null)), Cron.Minutely());
        });

        hangfireRouteBuilder.MapGet("/removeSampleJob", () =>
        {
            var manager = new RecurringJobManager();

            manager.RemoveIfExists("SampleJob");
        });

        nlogRouteBuilder.MapGet("/tryNLog", () =>
        {
            logger.LogError("This is a critical good sample");

            return Results.Ok();
        });

        compressingRouteBuilder.MapGet("/tryCompression", async () =>
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

        compressingRouteBuilder.MapGet("/tryDecompression", async (string compressedData) =>
        {
            var decompressedData = await new MyCompressingService().Decompress(compressedData);

            return decompressedData;
        });

        nlogRouteBuilder.MapGet("/logsWithEntityFrameworkAndLinq", async (int page, int pageSize, IDbContextFactory<MinimalApisDbContext> dbContextFactory) =>
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

        nlogRouteBuilder.MapGet("/logsWithDapperAndSql", async () =>
        {
            var stopwatch = Stopwatch.StartNew();

            using var connection = new SqlConnection(app.Configuration.GetConnectionString("MinimalAPIsDB"));
            var logs = (await connection.QueryAsync<Nlog>("SELECT * FROM NLog WHERE 1 = @param ",
                new { param = (int?)1 })).ToList();

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;

            return Results.Ok(new { elapsedTime, logs });
        });

        httpClientRouteBuilder.MapGet("/callAnotherEndpoint", async (HttpContext context) =>
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

        httpClientRouteBuilder.MapGet("/callAnotherEndpointOptimized", async (HttpContext context) =>
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
                return Results.Problem(problemDetail, statusCode: (int)response.StatusCode, title: response.ReasonPhrase);
            }
        });

        httpClientRouteBuilder.MapGet("/callThisEndpoint", () => Results.Ok(new { res = "This is the response from httpClient/getThisEndpoint !" }));

        mediatrRouteBuilder.MapGet("/getNotified", async (IMediator mediator) =>
        {
            await mediator.Publish(new Notification("Hello!"));

            return Results.Ok("Notified");
        });

        mediatrRouteBuilder.MapGet("/sendRequest", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new MyRequest("How are you?"));

            return Results.Ok(response);
        });

        fluentValidation.MapPost("/tryValidation", async ([FromBody] Customer customer) =>
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

public class Customer
{
    public string Surname { get; set; }
    public string Forename { get; set; }
    public string FiscalCode { get; set; }
    public string Birthday { get; set; }
    public bool HasDiscount { get; set; } = false;
    public int Discount { get; set; }
    public string Address { get; set; }
    public string Postcode { get; set; }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
        RuleFor(x => x.FiscalCode).Matches(x => @"^(?:[A-Z][AEIOU][AEIOUX]|[AEIOU]X{2}|[B-DF-HJ-NP-TV-Z]{2}[A-Z]){2}(?:[\dLMNP-V]{2}(?:[A-EHLMPR-T](?:[04LQ][1-9MNP-V]|[15MR][\dLMNP-V]|[26NS][0-8LMNP-U])|[DHPS][37PT][0L]|[ACELMRT][37PT][01LM]|[AC-EHLMPR-T][26NS][9V])|(?:[02468LNQSU][048LQU]|[13579MPRTV][26NS])B[26NS][9V])(?:[A-MZ][1-9MNP-V][\dLMNP-V]{2}|[A-M][0L](?:[1-9MNP-V][\dLMNP-V]|[0L][1-9MNP-V]))[A-Z]$");
        RuleFor(x => x.Birthday).Matches(x => @"^(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))$");
        RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);
        RuleFor(x => x.Address).Length(7, 250);
        RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage("Please specify a valid postcode");
    }

    private bool BeAValidPostcode(string postcode)
    {
        return true;
    }
}

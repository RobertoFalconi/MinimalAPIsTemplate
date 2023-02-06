global using Hangfire;
global using Hangfire.Common;
global using Hangfire.Dashboard;
global using Hangfire.MemoryStorage;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.Net.Http.Headers;
global using Microsoft.OpenApi.Models;
global using MinimalAPIs.Handlers;
global using MinimalAPIs.Models.DB;
global using MinimalAPIs.Properties;
global using MinimalAPIs.Services;
global using NLog;
global using NLog.Extensions.Logging;
global using NLog.Web;
global using System.Diagnostics;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO.Compression;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Text.Json;

// Create the app builder.
var builder = WebApplication.CreateBuilder(args);

// Add configurations to the container.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add loggers to the container.
builder.Logging.AddJsonConsole();
builder.Logging.AddNLogWeb(new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog")));
LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

// Parameters.
var connectionString = builder.Configuration.GetConnectionString("MinimalAPIsDB") ?? "";
var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var audience = builder.Configuration["Jwt:Audience"]!;
var issuer = builder.Configuration["Jwt:Issuer"]!;
var signingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
var encryptingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
//File.WriteAllBytes(builder.Configuration["Certificate:Path"]!, signingCertificate.Export(X509ContentType.Pfx, builder.Configuration["Certificate:Password"]));
var signingCertificateKey = new X509SecurityKey(signingCertificate);
var encryptingCertificateKey = new X509SecurityKey(encryptingCertificate);
var signingKeys = new List<SecurityKey> { symmetricKey, signingCertificateKey };

// Add API services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddHealthChecks();

// Add AuthZ and AuthN services.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKeys = signingKeys,
        TokenDecryptionKeys = new List<SecurityKey>
        {
            new EncryptingCredentials(symmetricKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512).Key,
            new EncryptingCredentials(encryptingCertificateKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512).Key
        },
        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
        {
            return signingKeys;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new MyAuthorizationRequirement()));
});

// Add DB services.
builder.Services.AddDbContext<MinimalApisDbContext>(options => options.UseSqlServer(connectionString));

// Add performance booster services.
builder.Services.AddResponseCompression();
builder.Services.AddRequestDecompression();

// Add custom services to the container.
builder.Services.AddScoped<IAuthorizationHandler, MyAuthorizationHandler>();
builder.Services.AddScoped<MyEndpointHandler>();

// Add third-parties services to the container.
builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
JobStorage.Current = new MemoryStorage();

// Add the app to the container.
var app = builder.Build();

// Map the endpoints.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<MyEndpointHandler>()?.RegisterAPIs(app, issuer, audience, symmetricKey, signingCertificateKey, encryptingCertificateKey);
}

// Configure the HTTP request pipeline.
app.UseResponseCompression();
app.UseRequestDecompression();
app.UseHsts();
app.UseHttpsRedirection();
app.MapHealthChecks("/Health");
app.UseHangfireDashboard("/Hangfire/Dashboard", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    _ = app.UseDeveloperExceptionPage();
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}
else
{
    // Error handling.
    _ = app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        AllowStatusCode404Response = true,
        ExceptionHandler = async (HttpContext context) =>
        {
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionHandlerFeature?.Error is BadHttpRequestException badRequestEx)
            {
                context.Response.StatusCode = badRequestEx.StatusCode;
            }

            if (context.Request.AcceptsJson()
                && context.RequestServices.GetRequiredService<IProblemDetailsService>() is { } problemDetailsService)
            {
                await problemDetailsService.WriteAsync(new()
                {
                    HttpContext = context,
                    AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                    ProblemDetails = { Status = context.Response.StatusCode }
                });
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var message = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode) switch
                {
                    { Length: > 0 } reasonPhrase => reasonPhrase,
                    _ => "An error occurred"
                };
                await context.Response.WriteAsync(message + "\r\n");
                await context.Response.WriteAsync($"Request ID: {Activity.Current?.Id ?? context.TraceIdentifier}");
            }
        }
    });
}

// Run the app.
app.Run();
global using Dapper;
global using FluentValidation;
global using Hangfire;
global using Hangfire.Common;
global using Hangfire.Dashboard;
global using Hangfire.SqlServer;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using MinimalAPIs.Endpoints;
global using MinimalAPIs.Filters;
global using MinimalAPIs.Handlers.CommandHandlers;
global using MinimalAPIs.Handlers.ConfigurationHandlers;
global using MinimalAPIs.Handlers.QueryHandlers;
global using MinimalAPIs.Models.API;
global using MinimalAPIs.Models.DB;
global using MinimalAPIs.Services;
global using NLog;
global using NLog.Extensions.Logging;
global using NLog.Web;
global using System.Diagnostics;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO.Compression;
global using System.Reflection;
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
var logger = LoggerFactory.Create(builder => builder.AddNLog().AddJsonConsole()).CreateLogger<Program>();
logger.LogInformation("So it begins");

// Add parameters to the container.
var connectionString = builder.Configuration.GetConnectionString("MinimalAPIsDB") ?? "";
var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var audience = builder.Configuration["Jwt:Audience"]!;
var issuer = builder.Configuration["Jwt:Issuer"]!;
var signingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
var encryptingCertificate = new CertificateRequest("cn=foobar", RSA.Create(), HashAlgorithmName.SHA512, RSASignaturePadding.Pss).CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1));
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
    options.IncludeErrorDetails = true;
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
        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) => signingKeys
    };
});
builder.Services.AddAuthorization(options => options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new AuthorizationRequirement())));

// Add DB services.
builder.Services.AddDbContextFactory<MinimalApisDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add performance booster services.
builder.Services.AddHttpClient();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddResponseCompression();
builder.Services.AddRequestDecompression();

// Add third-parties services to the container.
builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
builder.Services.AddHangfireServer();

if (builder.Environment.IsDevelopment())
{
    // Move services you want to use in development-only here.
}

// Add custom services to the container.
builder.Services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
builder.Services.AddScoped<MyTokenService>();
builder.Services.AddScoped<MyCompressingService>();

// Add the app to the container.
var app = builder.Build();

// Map the endpoints.
app.MapMyEndpoint(issuer, audience, symmetricKey, signingCertificateKey, encryptingCertificateKey);
app.MapCustomerEndpoint();
app.MapHealthChecks("/Health");

// Configure the HTTP request pipeline.
app.UseResponseCompression();
app.UseRequestDecompression();
app.UseHsts();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/Hangfire/Dashboard", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        AllowStatusCode404Response = true,
        ExceptionHandler = async (HttpContext context) =>
        {
            var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            await context.Response.WriteAsJsonAsync(new { error = error?.Message ?? "Bad Request", statusCode = StatusCodes.Status400BadRequest });
        }
    });
}

// Run the app.
app.Run();
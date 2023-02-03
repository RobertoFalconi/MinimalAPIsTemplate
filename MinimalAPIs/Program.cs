global using Hangfire;
global using Hangfire.Common;
global using Hangfire.Dashboard;
global using Hangfire.MemoryStorage;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using MinimalAPIs.Filters;
global using MinimalAPIs.Handlers;
global using MinimalAPIs.Models.DB;
global using MinimalAPIs.Services;
global using System;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Security.Cryptography;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;

// Create the app builder.
var builder = WebApplication.CreateBuilder(args);

// Add configurations to the container.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add loggers to the container.
builder.Logging.AddJsonConsole();

// Parameters.
var connectionString = builder.Configuration.GetConnectionString("MinimalAPIsDB") ?? "";          // from Secrets.json ?? from appsettings.json
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var audience = builder.Configuration["Jwt:Audience"]!;
var issuer = builder.Configuration["Jwt:Issuer"]!;
var rsa = RSA.Create();
var req = new CertificateRequest("cn=foobar", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));
File.WriteAllBytes(builder.Configuration["Certificate:Path"]!, cert.Export(X509ContentType.Pfx, builder.Configuration["Certificate:Password"]));
var keyCert = new X509SecurityKey(cert);
var keys = new List<SecurityKey> { key, keyCert };

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKeys = keys,
        TokenDecryptionKeys = new List<SecurityKey> { new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512).Key },
        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
        {
            return keys;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAuthorized", policy => policy.Requirements.Add(new MyAuthorizationRequirement()));
});

// Add DB services
builder.Services.AddDbContext<MinimalApisDbContext>(options => options.UseSqlServer(connectionString));

// Add custom services to the container.
builder.Services.AddScoped<IAuthorizationHandler, MyAuthorizationHandler>();
builder.Services.AddScoped<MyTokenHandler>();

// Add third-parties services to the container.
builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
JobStorage.Current = new MemoryStorage();

// Add the app to the container.
var app = builder.Build();

// Map the endpoints.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<MyTokenHandler>()?.RegisterAPIs(app, issuer, audience, key, keyCert);
}

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.MapHealthChecks("/Health");

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    _ = app.UseDeveloperExceptionPage();
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
    _ = app.UseHangfireDashboard("/Hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() }
    });
}
else
{
    _ = app.UseExceptionHandler("/Error");
}

// Run the app.
app.Run();
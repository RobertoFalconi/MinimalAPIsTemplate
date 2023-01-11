global using Hangfire;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using MinimalAPIs.Services;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using MinimalAPIs.Handlers;
using MinimalAPIs.Models;

var builder = WebApplication.CreateBuilder(args);

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var keyCert = new X509SecurityKey(new X509Certificate2(builder.Configuration["Certificate:Path"]!, builder.Configuration["Certificate:Password"]));
var keys = new List<SecurityKey> { key, keyCert };
var connectionString = builder.Configuration.GetConnectionString("connectionstring") ?? builder.Configuration["ConnectionString"]?.ToString() ?? "";          // from Secrets.json ?? from appsettings.json

builder.Services.AddDbContext<MinimalDbContext>(options => options.UseSqlServer(connectionString));
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
builder.Services.AddAuthorization();
builder.Logging.AddJsonConsole();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<MyTokenHandler>();
builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage()).AddHangfireServer();
JobStorage.Current = new MemoryStorage();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<MyTokenService>();
    scope.ServiceProvider.GetService<MyTokenHandler>()?.RegisterAPIs(app);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapHealthChecks("/healthz");
app.UseExceptionHandler("/error");

app.Map("/error", (HttpContext context) =>
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var errorMessage = error?.Message ?? "Unknown error";
    app.Logger.LogError(error, errorMessage);
    return Results.Json(data: errorMessage, statusCode: StatusCodes.Status400BadRequest);
}).ExcludeFromDescription();

app.Run();
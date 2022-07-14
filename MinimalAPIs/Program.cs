using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPIs.Handlers;
using MinimalAPIs.Models;
using MinimalAPIs.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
var keyCert = new X509SecurityKey(new X509Certificate2(builder.Configuration["Certificate:Path"], builder.Configuration["Certificate:Password"]));
var keys = new List<SecurityKey> { key, keyCert };

var connectionString = builder.Configuration.GetConnectionString("connectionstring") ?? // from Secrets.json
                                    builder.Configuration["ConnectionString"];          // from appsettings.json

builder.Services.AddDbContext<MinimalDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKeys = keys,
        TokenDecryptionKey = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512).Key
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var prova = context?.Principal?.Claims?.FirstOrDefault(x => x.Type == "custom")?.Value;
            if (prova == null)
            {
                context?.Fail("Unauthorized");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Logging.AddJsonConsole();

builder.Services.AddHealthChecks();

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

builder.Services.AddScoped<IMyTokenService, MyTokenService>();
builder.Services.AddScoped<MyTokenHandler>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BT Web API JSON v1");
        options.SwaggerEndpoint("/swagger/v1/swagger.yaml", "BT Web API YAML v1");
    });
}
else
{
    app.UseExceptionHandler("/error");
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<MyTokenHandler>()?
        .RegisterAPIs(app);
}

app.MapGet("/error", () => "An error occurred.");

app.MapHealthChecks("/healthz");

app.Run();
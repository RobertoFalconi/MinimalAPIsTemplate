using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPIs.Models;
using MinimalAPIs.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("connectionstring") ?? // from Secrets.json
                                    builder.Configuration["ConnectionString"];          // from appsettings.json

builder.Services.AddDbContext<MinimalDbContext>(options => options.UseSqlServer(connectionString));

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));

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
        IssuerSigningKey = key,
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

builder.Services.AddScoped<IMinimalService, MinimalService>();

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

    app.MapGet("/generateToken", () =>
    {
        var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature));
        var jwtPayload = new JwtPayload(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], null, null, DateTime.Now.AddMinutes(30), null);
        jwtPayload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload));
    });

    app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

    app.MapGet("/generateTokenEncrypted", () =>
    {
        var ep = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], null, null, DateTime.Now.AddHours(1), null, new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature), ep);
        token.Payload.AddClaim(new System.Security.Claims.Claim("custom", "prova"));
        return new JwtSecurityTokenHandler().WriteToken(token);
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.MapGet("/getDouble", (IMinimalService minimalService, int a) => minimalService.Double(a, new CancellationToken()));

app.MapGet("/error", () => "An error happened.");

app.MapHealthChecks("/healthz");

app.Run();
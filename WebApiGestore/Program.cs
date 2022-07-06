using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

var key = new X509SecurityKey(new X509Certificate2(builder.Configuration["Certificate:Path"], builder.Configuration["Certificate:Password"]));
var connectionString = builder.Configuration["ConnectionString"];

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
        IssuerSigningKey = key
    };
});
builder.Services.AddAuthorization();

builder.Logging.AddJsonConsole();
builder.Services.AddHealthChecks();

//builder.Services.AddDbContext<yourDbContext>(options => options.UseSqlServer(connectionString));

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

app.MapGet("/generateToken", () =>
{
    var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.RsaSha256));
    var jwtPayload = new JwtPayload(builder.Configuration["Jwt:Issuer"], builder.Configuration["Jwt:Audience"], null, null, DateTime.Now.Add(new TimeSpan(0, 0, 1800)), null);
    return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwtHeader, jwtPayload));
});

app.MapGet("/tryToken", () => Results.Ok()).RequireAuthorization();

app.MapGet("/error", () => "An error happened.");

app.MapHealthChecks("/healthz");

app.Run();

public record JsonResponse(string ErrorMessage);
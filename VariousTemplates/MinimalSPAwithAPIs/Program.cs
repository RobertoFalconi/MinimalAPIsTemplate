global using AutoMapper;
global using Dapper;
global using FluentValidation;
global using MediatR;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using MinimalSPAwithAPIs.Behaviors;
global using MinimalSPAwithAPIs.Extensions;
global using MinimalSPAwithAPIs.Handlers.CommandHandlers;
global using MinimalSPAwithAPIs.Handlers.QueryHandlers;
global using MinimalSPAwithAPIs.Handlers.SecurityHandlers;
global using MinimalSPAwithAPIs.Middlewares;
global using MinimalSPAwithAPIs.Models.DB;
global using MinimalSPAwithAPIs.Models.DTO;
global using MinimalSPAwithAPIs.Models.Filters;
global using System.Data;
//global using Serilog;
global using System.Linq.Dynamic.Core;
global using System.Linq.Expressions;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});

builder.Services.AddValidators(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    configuration.AddOpenBehavior(typeof(NotificationBehavior<,>));
});

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod()
           .AllowAnyHeader()
           .AllowAnyOrigin();
}));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdmAuthenticationOptions.DefaultScheme;
    options.DefaultChallengeScheme = IdmAuthenticationOptions.DefaultScheme;
    options.DefaultAuthenticateScheme = IdmAuthenticationOptions.DefaultScheme;
})
    .AddScheme<IdmAuthenticationOptions, IdmAuthenticationHandler>(IdmAuthenticationOptions.DefaultScheme, null);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Role1Policy", policy =>
        policy.Requirements.Add(new CurrentProfileRequirement(Roles.Role1)));
builder.Services.AddSingleton<IAuthorizationHandler, CurrentProfileHandler>();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MinimalSPAwithAPIs")!);
});

builder.Services.AddScoped<IDbConnection>((sp) => new SqlConnection(builder.Configuration.GetConnectionString("CRSAPiattaformaERM")));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//builder.Host.UseSerilog((context, configuration) =>
//{
//    configuration.ReadFrom.Configuration(context.Configuration);
//});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
//app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseMiddleware<ErrorMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
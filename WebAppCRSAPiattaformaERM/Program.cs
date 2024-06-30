global using AutoMapper;
global using FluentValidation;
global using MediatR;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
//global using Serilog;
global using System.Linq.Dynamic.Core;
global using System.Linq.Expressions;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using MinimalSPAwithAPIs.Extensions;
global using MinimalSPAwithAPIs.Handlers.AuthenticationHandlers;
global using MinimalSPAwithAPIs.Handlers.CommandHandlers;
global using MinimalSPAwithAPIs.Handlers.BehaviorHandlers;
global using MinimalSPAwithAPIs.Handlers.QueryHandlers;
global using MinimalSPAwithAPIs.Models.DB;
global using MinimalSPAwithAPIs.Models.DTO;
global using MinimalSPAwithAPIs.Models.Filters;
global using MinimalSPAwithAPIs.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});

builder.Services.AddValidators(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.AddOpenBehavior(typeof(ValidatorHandler<,>));
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

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MinimalSPAwithAPIs")!);
});

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
app.UseMiddleware<ErrorHandler>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
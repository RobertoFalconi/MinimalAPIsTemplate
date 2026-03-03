using Microsoft.EntityFrameworkCore;
using MinimalAPIsAndCleanArchitecture.Core.Interfaces;
using MinimalAPIsAndCleanArchitecture.Endpoints;
using MinimalAPIsAndCleanArchitecture.Infrastructure.Data;
using MinimalAPIsAndCleanArchitecture.Infrastucture.Repositories;
using MinimalAPIsAndCleanArchitecture.Core.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

app.MapWeatherForecast();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var url = app.Urls.FirstOrDefault() ?? "http://localhost:5000";
        Process.Start(new ProcessStartInfo($"{url}/swagger") { UseShellExecute = true });
    });
}

app.Run();
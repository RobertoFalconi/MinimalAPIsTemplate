using MinimalAPIsAndCleanArchitecture;
using MinimalAPIsAndCleanArchitecture.Core;
using MinimalAPIsAndCleanArchitecture.Endpoints;
using MinimalAPIsAndCleanArchitecture.Infrastructure;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapWeatherForecast();

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

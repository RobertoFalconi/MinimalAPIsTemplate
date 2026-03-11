using MinimalAPIsAndCleanArchitecture;
using MinimalAPIsAndCleanArchitecture.Core;
using MinimalAPIsAndCleanArchitecture.Endpoints;
using MinimalAPIsAndCleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapWeatherForecast();

app.Run();

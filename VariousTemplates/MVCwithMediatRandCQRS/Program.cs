global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using MVCwithMediatRandCQRS.CommandHandlers;
global using MVCwithMediatRandCQRS.DbModels;
global using MVCwithMediatRandCQRS.Handlers;
global using MVCwithMediatRandCQRS.Middlewares;
global using MVCwithMediatRandCQRS.QueryHandlers;
global using MVCwithMediatRandCQRS.ViewModels;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Net;
global using System.Reflection;
global using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.AddOpenBehavior(typeof(NotificationHandler<,>));
});

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCwithMediatRandCQRS.DB.Main")!);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ErrorMiddleware>();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

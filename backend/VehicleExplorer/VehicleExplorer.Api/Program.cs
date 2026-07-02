using Carter;
using FluentValidation;
using Microsoft.Extensions.Caching.Hybrid;
using Scalar.AspNetCore;
using System.Reflection;
using VehicleExplorer.Api.Clients;
using VehicleExplorer.Api.Middlewares.Behaviors;
using VehicleExplorer.Api.Middlewares.Handler;
using VehicleExplorer.Api.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddCarter();

string[] webDomains = builder.Configuration["WebDomain"]!
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins(webDomains)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var nhtsaClientSection = builder.Configuration.GetSection("NhtsaClient");

builder.Services.AddHttpClient<INhtsaClient, NhtsaClient>(client =>
{
    client.BaseAddress = new Uri(nhtsaClientSection["BaseAddressURL"]
        ?? throw new InvalidOperationException("Missing NhtsaClient:BaseAddressURL"));
    client.Timeout = nhtsaClientSection.GetValue<TimeSpan>("Timeout");
}).AddStandardResilienceHandler();

var cacheSection = builder.Configuration.GetSection("Cache");


builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = cacheSection.GetValue<TimeSpan>("Expiration")
    };
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors("AllowAngularApp");

app.UseExceptionHandler(option => { });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}


app.MapHealthChecks("/health");

app.MapCarter();

app.Run();

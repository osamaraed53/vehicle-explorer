using Carter;
using Microsoft.Extensions.Caching.Hybrid;
using Scalar.AspNetCore;
using System.Reflection;
using VehicleExplorer.Api.Clients;
using VehicleExplorer.Api.Sevices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddCarter();

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
});

builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.MapCarter();

app.Run();

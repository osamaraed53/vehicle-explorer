using Microsoft.Extensions.Caching.Hybrid;
using System.Reflection;
using VehicleExplorer.Api.Clients;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
        Expiration = nhtsaClientSection.GetValue<TimeSpan>("Expiration")
    };
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

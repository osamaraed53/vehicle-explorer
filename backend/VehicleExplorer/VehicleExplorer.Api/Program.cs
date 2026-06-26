using VehicleExplorer.Api.Clients;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var section = builder.Configuration.GetSection("NhtsaClient");

builder.Services.AddHttpClient<INhtsaClient, NhtsaClient>(client =>
{
    client.BaseAddress = new Uri(section["BaseAddressURL"]
        ?? throw new InvalidOperationException("Missing NhtsaClient:BaseAddressURL"));
    client.Timeout = section.GetValue<TimeSpan>("Timeout");
}).AddStandardResilienceHandler();


builder.Services.AddHybridCache();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

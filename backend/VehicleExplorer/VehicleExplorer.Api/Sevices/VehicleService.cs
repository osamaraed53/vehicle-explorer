using Microsoft.Extensions.Caching.Hybrid;
using VehicleExplorer.Api.Clients;
using VehicleExplorer.Api.DTOs;

namespace VehicleExplorer.Api.Sevices;

public sealed class VehicleService(INhtsaClient client, HybridCache cache) : IVehicleService
{
    private static readonly HybridCacheEntryOptions CacheOptions = new()
    {
        Expiration = TimeSpan.FromHours(12),
    };
    public async Task<IReadOnlyList<MakeDTO>> GetMakesAsync(CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync("makes", async (cancellationToken) =>
        {
            var makes = await client.GetAllMakesAsync(cancellationToken);
            return (IReadOnlyList<MakeDTO>)[.. makes
                .Select(m => new MakeDTO() {Id = m.MakeId,Name= m.MakeName })
                .OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase)];
        }, CacheOptions, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<ModelDTO>> GetModelsAsync(int makeId, int year, string? vehicleType, CancellationToken cancellationToken)
    {
        var typeKey = string.IsNullOrWhiteSpace(vehicleType) ? "" : vehicleType.Trim().ToLowerInvariant();

        return await cache.GetOrCreateAsync($"models:{makeId}:{year}:{typeKey}", async cancellationToken =>
        {
            var models = await client.GetModelsAsync(makeId, year, vehicleType, cancellationToken);

            return (IReadOnlyList<ModelDTO>)[.. models
                .Select(m => new ModelDTO() {Id = m.ModelId,Name=  m.ModelName,MakeId= m.MakeId,MakeName= m.MakeName })
                .OrderBy(m => m.Name, StringComparer.OrdinalIgnoreCase)];

        }, CacheOptions, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<VehicleTypeDTO>> GetVehicleTypesAsync(int makeId, CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync($"vehicle-types:{makeId}", async cancellationToken =>
        {
            var types = await client.GetVehicleTypesForMakeAsync(makeId, cancellationToken);
            return (IReadOnlyList<VehicleTypeDTO>)[.. types
                .Select(t => new VehicleTypeDTO() { Id=  t.VehicleTypeId,Name=  t.VehicleTypeName })
                .OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase)];

        }, CacheOptions, cancellationToken: cancellationToken);
    }
}

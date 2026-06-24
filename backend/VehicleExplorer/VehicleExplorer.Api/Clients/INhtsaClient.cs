using VehicleExplorer.Api.Clients.NhtsaResponse;

namespace VehicleExplorer.Api.Clients;

public interface INhtsaClient
{
    Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync();

    Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId);

    Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleTypet);

}

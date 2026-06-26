using VehicleExplorer.Api.Clients.NhtsaResponse;

namespace VehicleExplorer.Api.Clients;

public interface INhtsaClient
{
    Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId,CancellationToken cancellationToken);

    Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleTypet, CancellationToken cancellationToken);

}

using VehicleExplorer.Api.Clients.NhtsaResponse;

namespace VehicleExplorer.Api.Clients;

public class NhtsaClient : INhtsaClient
{
    public Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleTypet)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId)
    {
        throw new NotImplementedException();
    }
}

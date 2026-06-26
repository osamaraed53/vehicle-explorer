using VehicleExplorer.Api.Clients.NhtsaResponse;

namespace VehicleExplorer.Api.Clients;

public class NhtsaClient(HttpClient httpClient) : INhtsaClient
{
    public async Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<MakeResponse>>("getallmakes?format=json", cancellationToken);
        return response?.Results ?? [];
    }

    public async Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleType, CancellationToken cancellationToken)
    {
        var url = $"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}";
        if (!string.IsNullOrWhiteSpace(vehicleType))
        {
            url += $"/vehicletype/{Uri.EscapeDataString(vehicleType)}";
        }
        url += "?format=json";

        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<ModelResponse>>(url, cancellationToken);
        return response?.Results ?? [];
    }

    public async  Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId, CancellationToken cancellationToken)
    {

        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<VehicleTypeResponse>>($"GetVehicleTypesForMakeId/{makeId}?format=json",cancellationToken);
        return response?.Results ?? [];


    }
}

using VehicleExplorer.Api.Clients.NhtsaResponse;

namespace VehicleExplorer.Api.Clients;

public class NhtsaClient(HttpClient httpClient) : INhtsaClient
{
    public async Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync()
    {
        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<MakeResponse>>("getallmakes?format=json");
        return response?.Results ?? [];
    }

    public async Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleType)
    {
        var url = $"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}";
        if (!string.IsNullOrWhiteSpace(vehicleType))
        {
            url += $"/vehicletype/{Uri.EscapeDataString(vehicleType)}";
        }
        url += "?format=json";

        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<ModelResponse>>(url);
        return response?.Results ?? [];
    }

    public async  Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId)
    {

        var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<VehicleTypeResponse>>($"GetVehicleTypesForMakeId/{makeId}?format=json");
        return response?.Results ?? [];


    }
}

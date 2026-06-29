using System.Text.Json;
using VehicleExplorer.Api.Clients.NhtsaResponse;
using VehicleExplorer.Api.Exceptions;

namespace VehicleExplorer.Api.Clients;

public class NhtsaClient(HttpClient httpClient) : INhtsaClient
{
    public Task<IReadOnlyList<MakeResponse>> GetAllMakesAsync(CancellationToken cancellationToken)
        => GetResultsAsync<MakeResponse>("getallmakes?format=json", cancellationToken);

    public Task<IReadOnlyList<ModelResponse>> GetModelsAsync(int makeId, int year, string? vehicleType, CancellationToken cancellationToken)
    {
        var url = $"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}";
        if (!string.IsNullOrWhiteSpace(vehicleType))
        {
            url += $"/vehicletype/{Uri.EscapeDataString(vehicleType)}";
        }
        url += "?format=json";

        return GetResultsAsync<ModelResponse>(url, cancellationToken);
    }

    public Task<IReadOnlyList<VehicleTypeResponse>> GetVehicleTypesForMakeAsync(int makeId, CancellationToken cancellationToken)
        => GetResultsAsync<VehicleTypeResponse>($"GetVehicleTypesForMakeId/{makeId}?format=json", cancellationToken);

    private async Task<IReadOnlyList<T>> GetResultsAsync<T>(string url, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<GenericNhtsaResponse<T>>(url, cancellationToken);
            return response?.Results ?? [];
        }
        catch (Exception ex) 
        {
            throw new ClientException($"the request to the NHTSA service failed", ex)
            {
                RequestUri = url,
            };
        }
    }
}

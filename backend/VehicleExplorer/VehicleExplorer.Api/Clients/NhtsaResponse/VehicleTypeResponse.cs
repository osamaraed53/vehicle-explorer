using System.Text.Json.Serialization;

namespace VehicleExplorer.Api.Clients.NhtsaResponse;

public sealed class VehicleTypeResponse
{
    [JsonPropertyName("VehicleTypeId")]
    public int VehicleTypeId { get; set; }

    [JsonPropertyName("VehicleTypeName")]
    public string VehicleTypeName { get; set; } = string.Empty;
}

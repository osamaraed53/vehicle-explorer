using System.Text.Json.Serialization;

namespace VehicleExplorer.Api.Clients.NhtsaResponse;

public sealed class MakeResponse
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Make_Name")]
    public string MakeName { get; set; } = string.Empty;
}

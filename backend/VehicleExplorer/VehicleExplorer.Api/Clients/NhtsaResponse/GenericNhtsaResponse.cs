namespace VehicleExplorer.Api.Clients.NhtsaResponse;

public sealed class GenericNhtsaResponse<T>
{
    public int Count { get; set; }

    public string? Message { get; set; }

    public List<T> Results { get; set; } = [];
}

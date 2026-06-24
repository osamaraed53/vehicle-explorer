namespace VehicleExplorer.Api.Clients.NhtsaResponse;

public sealed class GeneralNhtsaResponse<T>
{
    public int Count { get; set; }

    public string? Message { get; set; }

    public List<T> Results { get; set; } = [];
}

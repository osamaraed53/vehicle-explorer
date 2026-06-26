namespace VehicleExplorer.Api.DTOs;

public class ModelDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
    public long MakeId { get; init; }
    public string MakeName { get; init; } = default!;
}

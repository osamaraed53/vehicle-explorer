namespace VehicleExplorer.Api.DTOs;

public record VehicleTypeDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
}

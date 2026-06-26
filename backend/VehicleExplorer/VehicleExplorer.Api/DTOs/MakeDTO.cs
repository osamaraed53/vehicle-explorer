namespace VehicleExplorer.Api.DTOs;

public record MakeDTO
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
}

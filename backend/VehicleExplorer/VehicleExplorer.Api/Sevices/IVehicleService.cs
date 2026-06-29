using VehicleExplorer.Api.DTOs;

namespace VehicleExplorer.Api.Sevices;

public interface IVehicleService
{
    Task<IReadOnlyList<MakeDTO>> GetMakesAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<VehicleTypeDTO>> GetVehicleTypesAsync(int makeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ModelDTO>> GetModelsAsync(int makeId, int year, string? vehicleType, CancellationToken cancellationToken);
}

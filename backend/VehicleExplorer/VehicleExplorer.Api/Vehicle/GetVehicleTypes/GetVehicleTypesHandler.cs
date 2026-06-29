using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Sevices;

namespace VehicleExplorer.Api.Vehicle.GetVehicleTypes;


public record GetVehicleTypesRequest([FromRoute] int MakeId, int Page, int PageSize) : IRequest<GetVehicleTypesResponse>;

public record GetVehicleTypesResponse(IReadOnlyList<VehicleTypeDTO> Data , int Count);

public class GetVehicleTypesHandler(IVehicleService vehicleService) : IRequestHandler<GetVehicleTypesRequest, GetVehicleTypesResponse>
{
    public async Task<GetVehicleTypesResponse> Handle(GetVehicleTypesRequest request, CancellationToken cancellationToken)
    {
        var entites = await vehicleService.GetVehicleTypesAsync(request.MakeId,cancellationToken);

        var data = entites.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        return new GetVehicleTypesResponse([.. data], entites.Count);
    }
}

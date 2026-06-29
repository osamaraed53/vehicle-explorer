using MediatR;
using Microsoft.AspNetCore.Mvc;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Sevices;

namespace VehicleExplorer.Api.Vehicle.GetModels;


public record GetModelsRequest([FromRoute]int MakeId, int Year, string? VehicleType, int Page, int PageSize) : IRequest<GetModelsResponse>;

public record GetModelsResponse(IReadOnlyList<ModelDTO> Data, int Count);

public class GetModelsHandler(IVehicleService vehicleService) : IRequestHandler<GetModelsRequest, GetModelsResponse>
{
    public async Task<GetModelsResponse> Handle(GetModelsRequest request, CancellationToken cancellationToken)
    {
        var entites = await vehicleService.GetModelsAsync(request.MakeId,request.Year,request.VehicleType,cancellationToken);

        var data = entites.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        return new GetModelsResponse([.. data], entites.Count);
    }
}

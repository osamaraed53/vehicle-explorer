using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Api.Vehicle.GetMakes;


public record GetMakesRequest(int Page, int PageSize) : IRequest<GetMakesResponse>;

public record GetMakesResponse(IReadOnlyList<MakeDTO> Data , int Count);

public class GetMakesHandler(IVehicleService vehicleService) : IRequestHandler<GetMakesRequest, GetMakesResponse>
{
    public async Task<GetMakesResponse> Handle(GetMakesRequest request, CancellationToken cancellationToken)
    {
        var entites = await vehicleService.GetMakesAsync(cancellationToken);

        var data = entites.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        return new GetMakesResponse([.. data], entites.Count);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Api.Vehicle.GetMakes;


public record GetMakesRequest(int Page, int PageSize, string? Search = null ) : IRequest<GetMakesResponse>;

public record GetMakesResponse(IReadOnlyList<MakeDTO> Data , int Count);

public class GetMakesHandler(IVehicleService vehicleService) : IRequestHandler<GetMakesRequest, GetMakesResponse>
{
    public async Task<GetMakesResponse> Handle(GetMakesRequest request, CancellationToken cancellationToken)
{
        var entites = await vehicleService.GetMakesAsync(cancellationToken);
        IReadOnlyList<MakeDTO> matches = entites;
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            matches = [.. entites.Where(m => m.Name.Contains(term, StringComparison.OrdinalIgnoreCase))];
        }

        var data = matches.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        return new GetMakesResponse([.. data], matches.Count);
    }
}

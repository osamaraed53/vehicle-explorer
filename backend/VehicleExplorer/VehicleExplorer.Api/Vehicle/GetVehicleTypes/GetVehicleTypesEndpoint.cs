using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VehicleExplorer.Api.Vehicle.GetVehicleTypes;

public class GetVehicleTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("makes/{makeId:int}/vehicle-types", HandleAsync)
        .WithName("Get all vehicle types")
        .Produces<GetVehicleTypesResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all vehicle types")
        .WithDescription("Get all vehicle types");
    }

    public static async Task<Ok<GetVehicleTypesResponse>> HandleAsync(
        [AsParameters] GetVehicleTypesRequest request,
        ISender sender)
    {
        var result = await sender.Send(request);

        return TypedResults.Ok(result);
    }
}

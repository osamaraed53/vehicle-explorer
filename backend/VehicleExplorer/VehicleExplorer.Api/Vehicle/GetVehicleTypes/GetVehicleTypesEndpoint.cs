using Carter;
using MediatR;

namespace VehicleExplorer.Api.Vehicle.GetVehicleTypes;

public class GetVehicleTypesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("makes/{makeId:int}/vehicle-types", async ([AsParameters] GetVehicleTypesRequest request, ISender sender) =>
        {

            var result = await sender.Send(request);

            return Results.Ok(result);

        }).WithName("Get all vehicle types")
        .Produces<GetVehicleTypesResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all vehicle types")
        .WithDescription("Get all vehicle types");

    }
}

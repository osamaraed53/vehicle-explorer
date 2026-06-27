using Carter;
using MediatR;

namespace VehicleExplorer.Api.Vehicle.GetMakes;

public class GetMakesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/makes", async (GetMakesRequest request, ISender sender) => {

            var result = await sender.Send(request);

            return Results.Ok(result);

        }).WithName("Get all makes")
        .Produces<GetMakesResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all makes")
        .WithDescription("Get all makes");
    }
}

using Carter;
using MediatR;

namespace VehicleExplorer.Api.Vehicle.GetModels;


public class GetModelsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/makes/{makeId:int}/models", async ([AsParameters] GetModelsRequest request, ISender sender) => {

            var result = await sender.Send(request);

            return Results.Ok(result);

        }).WithName("Get all models")
        .Produces<GetModelsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all models")
        .WithDescription("Get all models");
    }
}

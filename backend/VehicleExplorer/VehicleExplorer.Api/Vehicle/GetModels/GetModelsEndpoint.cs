using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VehicleExplorer.Api.Vehicle.GetModels;


public class GetModelsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/makes/{makeId:int}/models", HandleAsync)
        .WithName("Get all models")
        .Produces<GetModelsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all models")
        .WithDescription("Get all models");
    }

    public static async Task<Ok<GetModelsResponse>> HandleAsync(
        [AsParameters] GetModelsRequest request,
        ISender sender)
    {
        var result = await sender.Send(request);

        return TypedResults.Ok(result);
    }
}

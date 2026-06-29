using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VehicleExplorer.Api.Vehicle.GetMakes;

public class GetMakesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/makes", HandleAsync)
        .WithName("Get all makes")
        .Produces<GetMakesResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all makes")
        .WithDescription("Get all makes");
    }

    public static async Task<Ok<GetMakesResponse>> HandleAsync(
        [AsParameters] GetMakesRequest request,
        ISender sender)
    {
        var result = await sender.Send(request);

        return TypedResults.Ok(result);
    }
}

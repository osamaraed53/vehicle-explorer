using MediatR;
using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Vehicle.GetVehicleTypes;

namespace VehicleExplorer.Tests.Endpoints;

public class GetVehicleTypesEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task HandleAsync_sends_the_request_through_the_mediator_and_returns_ok_with_the_response()
    {
        var request = new GetVehicleTypesRequest(MakeId: 441, Page: 1, PageSize: 10);
        var expected = new GetVehicleTypesResponse(
            [new VehicleTypeDTO { Id = 2, Name = "Passenger Car" }],
            Count: 1);
        _sender.Send(request, Arg.Any<CancellationToken>()).Returns(expected);

        var result = await GetVehicleTypesEndpoint.HandleAsync(request, _sender);

        result.Value.ShouldBe(expected);
        await _sender.Received(1).Send(request, Arg.Any<CancellationToken>());
    }
}

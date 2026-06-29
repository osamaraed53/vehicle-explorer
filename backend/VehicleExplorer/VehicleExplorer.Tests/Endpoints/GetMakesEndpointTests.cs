using MediatR;
using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Vehicle.GetMakes;

namespace VehicleExplorer.Tests.Endpoints;

public class GetMakesEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task HandleAsync_sends_the_request_through_the_mediator_and_returns_ok_with_the_response()
    {
        var request = new GetMakesRequest(Page: 1, PageSize: 10);
        var expected = new GetMakesResponse(
            [new MakeDTO { Id = 440, Name = "Aston Martin" }],
            Count: 1);
        _sender.Send(request, Arg.Any<CancellationToken>()).Returns(expected);

        var result = await GetMakesEndpoint.HandleAsync(request, _sender);

        result.Value.ShouldBe(expected);
        await _sender.Received(1).Send(request, Arg.Any<CancellationToken>());
    }
}

using MediatR;
using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Vehicle.GetModels;

namespace VehicleExplorer.Tests.Endpoints;

public class GetModelsEndpointTests
{
    private readonly ISender _sender = Substitute.For<ISender>();

    [Fact]
    public async Task HandleAsync_sends_the_request_through_the_mediator_and_returns_ok_with_the_response()
    {
        var request = new GetModelsRequest(MakeId: 474, Year: 2025, VehicleType: null, Page: 1, PageSize: 100);
        var expected = new GetModelsResponse(
            [new ModelDTO { Id = 1, Name = "Model S", MakeId = 474, MakeName = "Tesla" }],
            Count: 1);
        _sender.Send(request, Arg.Any<CancellationToken>()).Returns(expected);

        var result = await GetModelsEndpoint.HandleAsync(request, _sender);

        result.Value.ShouldBe(expected);
        await _sender.Received(1).Send(request, Arg.Any<CancellationToken>());
    }
}

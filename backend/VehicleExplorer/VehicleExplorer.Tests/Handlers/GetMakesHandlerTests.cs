using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Api.Vehicle.GetMakes;

namespace VehicleExplorer.Tests.Handlers;

public class GetMakesHandlerTests
{
    private readonly IVehicleService _vehicleService = Substitute.For<IVehicleService>();

    private static IReadOnlyList<MakeDTO> Makes(int count) =>
        [.. Enumerable.Range(1, count).Select(i => new MakeDTO { Id = i, Name = $"Make {i:000}" })];

    [Fact]
    public async Task Returns_the_requested_page_and_the_total_count()
    {
        _vehicleService.GetMakesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Makes(30)));
        
        var handler = new GetMakesHandler(_vehicleService);

        var response = await handler.Handle(new GetMakesRequest(Page: 2, PageSize: 10), CancellationToken.None);

        response.Count.ShouldBe(30);
        response.Data.Count.ShouldBe(10);
        response.Data[0].Id.ShouldBe(11);
        response.Data[^1].Id.ShouldBe(20);
    }

    [Fact]
    public async Task Returns_an_empty_page_when_the_requested_page_is_beyond_the_data()
    {
        _vehicleService.GetMakesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Makes(5)));
        var handler = new GetMakesHandler(_vehicleService);

        var response = await handler.Handle(new GetMakesRequest(Page: 10, PageSize: 10), CancellationToken.None);

        response.Data.ShouldBeEmpty();
        response.Count.ShouldBe(5);
    }
}

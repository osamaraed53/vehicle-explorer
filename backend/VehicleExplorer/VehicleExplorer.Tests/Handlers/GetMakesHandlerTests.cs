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

    [Fact]
    public async Task Filters_by_search_term_case_insensitively_and_counts_only_matches()
    {
        _vehicleService.GetMakesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<MakeDTO>>(
            [
                new MakeDTO { Id = 1, Name = "Toyota" },
                new MakeDTO { Id = 2, Name = "Lexus" },
                new MakeDTO { Id = 3, Name = "Scion (Toyota)" },
            ]));
        var handler = new GetMakesHandler(_vehicleService);

        var response = await handler.Handle(
            new GetMakesRequest(Page: 1, PageSize: 10, Search: "toyota"),
            CancellationToken.None);

        response.Count.ShouldBe(2);
        response.Data.Select(m => m.Id).ShouldBe([1, 3]);
    }

    [Fact]
    public async Task Paginates_within_the_filtered_subset()
    {
        _vehicleService.GetMakesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Makes(30)));
        var handler = new GetMakesHandler(_vehicleService);

        var response = await handler.Handle(
            new GetMakesRequest(Page: 2, PageSize: 4, Search: "Make 01"),
            CancellationToken.None);

        response.Count.ShouldBe(10);
        response.Data.Count.ShouldBe(4);
    }
}

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.Clients;
using VehicleExplorer.Api.Clients.NhtsaResponse;
using VehicleExplorer.Api.Services;

namespace VehicleExplorer.Tests.Services;

public class VehicleServiceTests
{
    private readonly INhtsaClient _client = Substitute.For<INhtsaClient>();
    private static HybridCache CreateCache() =>
        new ServiceCollection().AddHybridCache().Services
            .BuildServiceProvider().GetRequiredService<HybridCache>();

    [Fact]
    public async Task GetMakesAsync_maps_responses_and_orders_by_name_ignoring_case()
    {
        _client.GetAllMakesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IReadOnlyList<MakeResponse>>(
        [
            new MakeResponse { MakeId = 1, MakeName = "Zephyr" },
            new MakeResponse { MakeId = 2, MakeName = "acura" },
        ]));
        var service = new VehicleService(_client, CreateCache());

        var makes = await service.GetMakesAsync(CancellationToken.None);

        makes.Select(make => make.Name).ShouldBe(["acura", "Zephyr"]);
        makes.First(make => make.Name == "acura").Id.ShouldBe(2);
    }

    [Fact]
    public async Task GetMakesAsync_caches_the_result_and_calls_the_client_only_once()
    {
        _client.GetAllMakesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IReadOnlyList<MakeResponse>>(
            [new MakeResponse { MakeId = 1, MakeName = "Acura" }]));
        var service = new VehicleService(_client, CreateCache());

        await service.GetMakesAsync(CancellationToken.None);
        await service.GetMakesAsync(CancellationToken.None);

        await _client.Received(1).GetAllMakesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetVehicleTypesAsync_maps_responses_and_orders_by_name()
    {
        _client.GetVehicleTypesForMakeAsync(440, Arg.Any<CancellationToken>()).Returns(Task.FromResult<IReadOnlyList<VehicleTypeResponse>>(
        [
            new VehicleTypeResponse { VehicleTypeId = 2, VehicleTypeName = "Truck" },
            new VehicleTypeResponse { VehicleTypeId = 1, VehicleTypeName = "Car" },
        ]));
        var service = new VehicleService(_client, CreateCache());

        var types = await service.GetVehicleTypesAsync(440, CancellationToken.None);

        types.Select(type => type.Name).ShouldBe(["Car", "Truck"]);
    }
}

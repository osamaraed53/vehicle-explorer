using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Api.Vehicle.GetVehicleTypes;

namespace VehicleExplorer.Tests.Handlers;

public class GetVehicleTypesHandlerTests
{
    private readonly IVehicleService _vehicleService = Substitute.For<IVehicleService>();

    [Fact]
    public async Task Forwards_the_make_id_to_the_service()
    {
        _vehicleService.GetVehicleTypesAsync(440, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<VehicleTypeDTO>>([]));
        var handler = new GetVehicleTypesHandler(_vehicleService);

        await handler.Handle(new GetVehicleTypesRequest(440, Page: 1, PageSize: 10), CancellationToken.None);

        await _vehicleService.Received(1).GetVehicleTypesAsync(440, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Paginates_the_vehicle_types_and_reports_the_total_count()
    {
        IReadOnlyList<VehicleTypeDTO> types =
            [.. Enumerable.Range(1, 12).Select(i => new VehicleTypeDTO { Id = i, Name = $"Type {i:000}" })];
        _vehicleService.GetVehicleTypesAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(types));
        var handler = new GetVehicleTypesHandler(_vehicleService);

        var response = await handler.Handle(new GetVehicleTypesRequest(440, Page: 1, PageSize: 5), CancellationToken.None);

        response.Count.ShouldBe(12);
        response.Data.Count.ShouldBe(5);
        response.Data[0].Id.ShouldBe(1);
    }
}

using NSubstitute;
using Shouldly;
using VehicleExplorer.Api.DTOs;
using VehicleExplorer.Api.Services;
using VehicleExplorer.Api.Vehicle.GetModels;

namespace VehicleExplorer.Tests.Handlers;

public class GetModelsHandlerTests
{
    private readonly IVehicleService _vehicleService = Substitute.For<IVehicleService>();

    [Fact]
    public async Task Forwards_request_parameters_to_the_service()
    {
        _vehicleService
            .GetModelsAsync(7, 2020, "truck", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ModelDTO>>([]));
        var handler = new GetModelsHandler(_vehicleService);

        await handler.Handle(new GetModelsRequest(7, 2020, "truck", Page: 1, PageSize: 10), CancellationToken.None);

        await _vehicleService.Received(1).GetModelsAsync(7, 2020, "truck", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Paginates_the_models_and_reports_the_total_count()
    {
        IReadOnlyList<ModelDTO> models =
            [.. Enumerable.Range(1, 15).Select(i => new ModelDTO { Id = i, Name = $"Model {i:000}", MakeId = 7, MakeName = "Acura" })];
        _vehicleService
            .GetModelsAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(models));
        var handler = new GetModelsHandler(_vehicleService);

        var response = await handler.Handle(new GetModelsRequest(7, 2020, null, Page: 2, PageSize: 10), CancellationToken.None);

        response.Count.ShouldBe(15);
        response.Data.Count.ShouldBe(5);
        response.Data[0].Id.ShouldBe(11);
    }
}

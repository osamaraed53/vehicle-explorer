using FluentValidation;

namespace VehicleExplorer.Api.Vehicle.GetVehicleTypes;

public sealed class GetVehicleTypesValidator : AbstractValidator<GetVehicleTypesRequest>
{
    public GetVehicleTypesValidator()
    {
        RuleFor(request => request.MakeId)
            .GreaterThan(0)
            .WithMessage("MakeId must be greater than 0.");

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}

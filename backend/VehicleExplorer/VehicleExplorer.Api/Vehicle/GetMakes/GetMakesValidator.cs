using FluentValidation;

namespace VehicleExplorer.Api.Vehicle.GetMakes;

public sealed class GetMakesValidator : AbstractValidator<GetMakesRequest>
{
    public GetMakesValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}

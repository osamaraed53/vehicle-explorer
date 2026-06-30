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

        RuleFor(request => request.Search)
            .MaximumLength(100)
            .WithMessage("Search must be 100 characters or fewer.")
            .When(request => request.Search is not null);
    }
}

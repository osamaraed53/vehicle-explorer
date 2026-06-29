using FluentValidation;

namespace VehicleExplorer.Api.Vehicle.GetModels;

// TODO :Create a constant file for error messages
public sealed class GetModelsValidator : AbstractValidator<GetModelsRequest>
{
    private static readonly int MaxModelYear = DateTime.UtcNow.Year + 1;
    public GetModelsValidator()
    {
        RuleFor(request => request.MakeId)
            .GreaterThan(0)
            .WithMessage("MakeId must be greater than 0.");

        RuleFor(request => request.Year)
            .InclusiveBetween(1900, MaxModelYear)
            .WithMessage($"Year must be betwween 1900 and {MaxModelYear}.");

        RuleFor(request => request.VehicleType)
            .MaximumLength(50)
            .WithMessage("VehicleType must be 50 characters or fewer.")
            .When(request => !string.IsNullOrWhiteSpace(request.VehicleType));

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}

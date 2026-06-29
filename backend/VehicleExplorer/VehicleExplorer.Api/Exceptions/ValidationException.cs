namespace VehicleExplorer.Api.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<ValidationFailureDto> Errors { get; private set; } = default!;

    public ValidationException(string message) : base(message) { }
    
    public ValidationException(IEnumerable<ValidationFailureDto> errors) : base(BuildErrorMessage(errors))
    {
        Errors = errors;
    }
    private static string BuildErrorMessage(IEnumerable<ValidationFailureDto> errors)
    {
        var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}");
        return "Validation failed: " + string.Join(string.Empty, arr);
    }

}
public class ValidationFailureDto
{
    public string PropertyName { get; set; } = default!;
    public string ErrorMessage { get; set; } = default!;
}
using FluentValidation;
using MediatR;
using VehicleExplorer.Api.Exceptions;
using ValidationException = VehicleExplorer.Api.Exceptions.ValidationException;

namespace VehicleExplorer.Api.Middlewares.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators):
    IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);
        
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var failures =
            validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .Select(failure => new ValidationFailureDto
            {
                PropertyName = failure.PropertyName,
                ErrorMessage = failure.ErrorMessage
            })
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}

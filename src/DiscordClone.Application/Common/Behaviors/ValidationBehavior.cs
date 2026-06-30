using ErrorOr;
using FluentValidation;
using MediatR;

namespace DiscordClone.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline step that runs any FluentValidation validator for the request before the
/// handler. Validation failures are converted to ErrorOr validation errors rather than thrown,
/// so the controller can map them to a 400 without exception-based control flow.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(failure => Error.Validation(failure.PropertyName, failure.ErrorMessage));

        // ErrorOr<T> defines an implicit conversion from List<Error>; dynamic dispatches to it.
        return (dynamic)errors;
    }
}

using ErrorOr;
using FluentValidation;
using MediatR;

namespace SweetMedical.Application.Common.Behaviors;

public class ValidateBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null):
    IPipelineBehavior<TRequest, TResponse>
    where TRequest: IRequest<TResponse>
    where TResponse: IErrorOr
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
        return (dynamic)validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation
                (validationFailure.PropertyName, validationFailure.ErrorMessage));
    }
}
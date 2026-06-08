using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SweetMedical.Api.Errors;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
        {
            await HandleDomainExceptionAsync(httpContext, domainException, cancellationToken);
            return true;
        }

        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
        };

        httpContext.Response.StatusCode = problem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static async Task HandleDomainExceptionAsync(
        HttpContext httpContext,
        DomainException exception,
        CancellationToken cancellationToken)
    {
        var errors = exception.Errors;

        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var validationErrors = errors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

            var validationProblem = new HttpValidationProblemDetails(validationErrors)
            {
                Status = StatusCodes.Status400BadRequest
            };

            httpContext.Response.StatusCode = validationProblem.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(validationProblem, cancellationToken);
            return;
        }

        var first = errors[0];
        var statusCode = first.Type switch
        {
            ErrorType.NotFound     => StatusCodes.Status404NotFound,
            ErrorType.Conflict     => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _                      => StatusCodes.Status500InternalServerError,
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = first.Description
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
    }
}

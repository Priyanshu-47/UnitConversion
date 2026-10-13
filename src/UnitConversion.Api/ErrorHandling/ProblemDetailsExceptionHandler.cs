using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Core.Exceptions;

namespace UnitConversion.Api.ErrorHandling;

/// <summary>
/// Global exception handler mapping exceptions to RFC 7807 problem details.
/// Domain errors become 400/422 with their client-safe message; anything else is
/// logged server-side and reported as a generic 500 so no stack traces or
/// internals ever reach the client.
/// </summary>
public sealed class ProblemDetailsExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ProblemDetailsExceptionHandler> _logger;

    /// <summary>Creates the handler.</summary>
    public ProblemDetailsExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<ProblemDetailsExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title, detail) = Map(exception);

        if (status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = status;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.com/{status}",
                Instance = httpContext.Request.Path,
            },
        });
    }

    private static (int Status, string Title, string Detail) Map(Exception exception) => exception switch
    {
        UnknownUnitException e => (StatusCodes.Status400BadRequest, "Unknown unit", e.Message),
        InvalidConversionInputException e => (StatusCodes.Status400BadRequest, "Invalid input", e.Message),
        BelowAbsoluteZeroException e => (StatusCodes.Status400BadRequest, "Value below absolute zero", e.Message),
        CategoryMismatchException e => (StatusCodes.Status422UnprocessableEntity, "Incompatible categories", e.Message),
        BadHttpRequestException e => (StatusCodes.Status400BadRequest, "Malformed request", e.Message),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "An unexpected error occurred while processing the request."),
    };
}

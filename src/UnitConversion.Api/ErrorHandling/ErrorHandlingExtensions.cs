using Microsoft.AspNetCore.Mvc;

namespace UnitConversion.Api.ErrorHandling;

/// <summary>Middleware wiring for error responses.</summary>
public static class ErrorHandlingExtensions
{
    /// <summary>
    /// Adds status-code pages that emit RFC 7807 problem details (e.g. for 404
    /// routes or 405 method mismatches) instead of empty bodies.
    /// </summary>
    public static IApplicationBuilder UseProblemDetailsStatusCodes(this IApplicationBuilder app) =>
        app.UseStatusCodePages(async statusContext =>
        {
            var httpContext = statusContext.HttpContext;
            var statusCode = httpContext.Response.StatusCode;

            if (statusCode < StatusCodes.Status400BadRequest || httpContext.Response.HasStarted)
            {
                return;
            }

            var problemDetailsService = httpContext.RequestServices.GetService<IProblemDetailsService>();
            if (problemDetailsService is null)
            {
                return;
            }

            httpContext.Response.StatusCode = statusCode;
            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = TitleFor(statusCode),
                    Type = $"https://httpstatuses.com/{statusCode}",
                    Instance = httpContext.Request.Path,
                },
            });
        });

    private static string TitleFor(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        StatusCodes.Status405MethodNotAllowed => "Method Not Allowed",
        StatusCodes.Status406NotAcceptable => "Not Acceptable",
        StatusCodes.Status415UnsupportedMediaType => "Unsupported Media Type",
        StatusCodes.Status500InternalServerError => "Internal Server Error",
        StatusCodes.Status501NotImplemented => "Not Implemented",
        StatusCodes.Status503ServiceUnavailable => "Service Unavailable",
        _ => "HTTP Error",
    };
}

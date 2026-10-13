using UnitConversion.Api.Contracts;
using UnitConversion.Core.Conversion;

namespace UnitConversion.Api.Endpoints;

/// <summary>Endpoint mappings for unit conversion.</summary>
public static class ConversionEndpoints
{
    /// <summary>Maps <c>GET /api/v1/convert</c> onto the provided route group.</summary>
    public static IEndpointRouteBuilder MapConversionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/convert", HandleConvert)
            .WithName("Convert")
            .WithSummary("Converts a value from one unit to another.")
            .WithDescription(
                "Both units must belong to the same category. Lookup is case-insensitive and accepts " +
                "symbols and aliases (for example 'm', 'meter', 'metre'). Optionally round the result " +
                "with precision (0-15 decimal places); no rounding by default.")
            .WithTags("Conversion")
            .Produces<ConvertResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return endpoints;
    }

    private static IResult HandleConvert(
        double value,
        string from,
        string to,
        int? precision,
        IConversionService conversion)
    {
        var result = conversion.Convert(value, from, to, precision);
        return Results.Ok(new ConvertResponse(result.Category, result.Value, result.From, result.To, result.Result));
    }
}

using UnitConversion.Api.Contracts;
using UnitConversion.Core.Catalog;

namespace UnitConversion.Api.Endpoints;

/// <summary>Endpoint mappings for browsing categories and units.</summary>
public static class CatalogEndpoints
{
    /// <summary>Maps <c>GET /api/v1/categories</c> and <c>GET /api/v1/categories/{category}/units</c>.</summary>
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/categories", HandleGetCategories)
            .WithName("GetCategories")
            .WithSummary("Lists all conversion categories.")
            .WithTags("Catalog")
            .Produces<CategoryResponse[]>();

        endpoints.MapGet("/categories/{category}/units", HandleGetUnits)
            .WithName("GetUnits")
            .WithSummary("Lists the units of a category, including names, symbols, and aliases.")
            .WithDescription("The category is matched case-insensitively by id, for example 'length' or 'flow-rate'.")
            .WithTags("Catalog")
            .Produces<UnitResponse[]>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static IResult HandleGetCategories(IUnitCatalog catalog) =>
        Results.Ok(catalog.GetCategories().Select(c => new CategoryResponse(c.Id, c.Name)).ToArray());

    private static IResult HandleGetUnits(string category, IUnitCatalog catalog)
    {
        var units = catalog.GetUnits(category);

        if (units is null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Category not found",
                detail: $"Unknown category '{category}'. List categories via GET /api/v1/categories.");
        }

        return Results.Ok(units.Select(u => new UnitResponse(u.Name, u.Symbol, u.Aliases)).ToArray());
    }
}

namespace UnitConversion.Core.Catalog;

/// <summary>A unit together with the category it belongs to.</summary>
public sealed record ResolvedUnit(UnitCategory Category, UnitDefinition Unit);

/// <summary>
/// Provides category and unit lookups. The API layer depends only on this
/// abstraction, so the in-memory implementation can later be replaced by a
/// database, JSON file, or admin-managed source without touching the endpoints.
/// </summary>
public interface IUnitCatalog
{
    /// <summary>Returns all known categories.</summary>
    IReadOnlyList<UnitCategory> GetCategories();

    /// <summary>Finds a category by id (case-insensitive), or <see langword="null"/> if unknown.</summary>
    UnitCategory? FindCategory(string id);

    /// <summary>Lists the units of a category id (case-insensitive), or <see langword="null"/> if the category is unknown.</summary>
    IReadOnlyList<UnitDefinition>? GetUnits(string id);

    /// <summary>Resolves a symbol or alias, case-insensitively; <see langword="null"/> when unknown.</summary>
    ResolvedUnit? Resolve(string symbolOrAlias);
}

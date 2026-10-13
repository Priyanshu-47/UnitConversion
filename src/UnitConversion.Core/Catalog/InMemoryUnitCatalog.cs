using UnitConversion.Core.Data;

namespace UnitConversion.Core.Catalog;

/// <summary>
/// In-memory catalog built from static per-category data definitions. Lookups are
/// case-insensitive over symbols and aliases; duplicate tokens fail fast at
/// construction so data mistakes surface at startup, not at request time.
/// Thread-safe: the catalog is immutable after construction.
/// </summary>
public sealed class InMemoryUnitCatalog : IUnitCatalog
{
    private readonly IReadOnlyList<UnitCategory> _categories;
    private readonly Dictionary<string, UnitCategory> _categoriesById;
    private readonly Dictionary<string, ResolvedUnit> _unitsByToken;

    /// <summary>Builds the default catalog from the bundled category definitions.</summary>
    public InMemoryUnitCatalog()
        : this(CreateDefaultCategories())
    {
    }

    /// <summary>
    /// Builds a catalog from explicit categories (custom or external data sources).
    /// Kept private so dependency injection can only ever construct the default
    /// catalog; DI would otherwise satisfy an <c>IEnumerable&lt;UnitCategory&gt;</c>
    /// constructor argument with an empty sequence.
    /// </summary>
    private InMemoryUnitCatalog(IEnumerable<UnitCategory> categories)
    {
        _categories = categories.ToList().AsReadOnly();
        _categoriesById = new Dictionary<string, UnitCategory>(StringComparer.OrdinalIgnoreCase);
        _unitsByToken = new Dictionary<string, ResolvedUnit>(StringComparer.OrdinalIgnoreCase);

        foreach (var category in _categories)
        {
            if (!_categoriesById.TryAdd(category.Id, category))
            {
                throw new InvalidOperationException($"Duplicate category id '{category.Id}'.");
            }

            foreach (var unit in category.Units)
            {
                if (!_unitsByToken.TryAdd(unit.Symbol.Trim(), new ResolvedUnit(category, unit)))
                {
                    throw new InvalidOperationException($"Duplicate unit symbol '{unit.Symbol}' in category '{category.Id}'.");
                }

                foreach (var alias in unit.Aliases)
                {
                    if (!_unitsByToken.TryAdd(alias.Trim(), new ResolvedUnit(category, unit)))
                    {
                        throw new InvalidOperationException($"Duplicate unit alias '{alias}' in category '{category.Id}'.");
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<UnitCategory> GetCategories() => _categories;

    /// <inheritdoc />
    public UnitCategory? FindCategory(string id) =>
        string.IsNullOrWhiteSpace(id) ? null : _categoriesById.GetValueOrDefault(id.Trim());

    /// <inheritdoc />
    public IReadOnlyList<UnitDefinition>? GetUnits(string id) => FindCategory(id)?.Units;

    /// <inheritdoc />
    public ResolvedUnit? Resolve(string symbolOrAlias) =>
        string.IsNullOrWhiteSpace(symbolOrAlias)
            ? null
            : _unitsByToken.GetValueOrDefault(symbolOrAlias.Trim());

    private static IReadOnlyList<UnitCategory> CreateDefaultCategories() => new UnitCategory[]
    {
        LengthUnits.Create(),
        TemperatureUnits.Create(),
        MassUnits.Create(),
        VolumeUnits.Create(),
        SpeedUnits.Create(),
        PressureUnits.Create(),
        FlowRateUnits.Create(),
        PowerUnits.Create(),
        TorqueUnits.Create(),
        TimeUnits.Create(),
    };
}

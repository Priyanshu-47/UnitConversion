using UnitConversion.Core.Catalog;
using UnitConversion.Core.Exceptions;

namespace UnitConversion.Core.Conversion;

/// <summary>
/// Default conversion pipeline: validate input, resolve both units
/// case-insensitively, verify they share a category, convert through the base
/// unit, enforce the category's base-unit floor (absolute zero), optionally round.
/// </summary>
public sealed class ConversionService : IConversionService
{
    private const int MaxPrecision = 15;

    private readonly IUnitCatalog _catalog;

    /// <summary>Creates the service over the given catalog.</summary>
    public ConversionService(IUnitCatalog catalog)
    {
        _catalog = catalog;
    }

    /// <inheritdoc />
    public ConversionResult Convert(double value, string from, string to, int? precision = null)
    {
        if (string.IsNullOrWhiteSpace(from))
        {
            throw new InvalidConversionInputException("Parameter 'from' is required.");
        }

        if (string.IsNullOrWhiteSpace(to))
        {
            throw new InvalidConversionInputException("Parameter 'to' is required.");
        }

        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new InvalidConversionInputException("Parameter 'value' must be a finite number; NaN and Infinity are not allowed.");
        }

        if (precision is < 0 or > MaxPrecision)
        {
            throw new InvalidConversionInputException($"Parameter 'precision' must be between 0 and {MaxPrecision}.");
        }

        var source = _catalog.Resolve(from) ?? throw new UnknownUnitException(from);
        var target = _catalog.Resolve(to) ?? throw new UnknownUnitException(to);

        if (!string.Equals(source.Category.Id, target.Category.Id, StringComparison.OrdinalIgnoreCase))
        {
            throw new CategoryMismatchException(source.Category.Name, target.Category.Name);
        }

        var baseValue = source.Unit.ToBase(value);

        if (source.Category.MinimumBaseValue is { } minimum && baseValue < minimum)
        {
            throw new BelowAbsoluteZeroException(source.Category.Name, minimum);
        }

        var result = target.Unit.FromBase(baseValue);

        if (precision is { } digits)
        {
            result = Math.Round(result, digits, MidpointRounding.AwayFromZero);
        }

        return new ConversionResult(source.Category.Id, value, source.Unit.Symbol, target.Unit.Symbol, result);
    }
}

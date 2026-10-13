namespace UnitConversion.Core.Exceptions;

/// <summary>
/// Base type for domain errors raised by the conversion pipeline. The API layer
/// maps derived types to HTTP status codes; messages are safe to show to clients.
/// </summary>
public abstract class ConversionException : Exception
{
    protected ConversionException(string message)
        : base(message)
    {
    }
}

/// <summary>Thrown when a unit symbol or alias cannot be resolved. Maps to HTTP 400.</summary>
public sealed class UnknownUnitException : ConversionException
{
    /// <summary>The token that could not be resolved.</summary>
    public string Symbol { get; }

    /// <summary>Creates the exception for an unresolvable unit token.</summary>
    public UnknownUnitException(string symbol)
        : base($"Unknown unit '{symbol}'. List available units via GET /api/v1/categories/{{category}}/units.")
    {
        Symbol = symbol;
    }
}

/// <summary>Thrown when the two units belong to different categories. Maps to HTTP 422.</summary>
public sealed class CategoryMismatchException : ConversionException
{
    /// <summary>Category of the source unit.</summary>
    public string FromCategory { get; }

    /// <summary>Category of the target unit.</summary>
    public string ToCategory { get; }

    /// <summary>Creates the exception naming both categories involved.</summary>
    public CategoryMismatchException(string fromCategory, string toCategory)
        : base($"Cannot convert between categories '{fromCategory}' and '{toCategory}': both units must belong to the same category.")
    {
        FromCategory = fromCategory;
        ToCategory = toCategory;
    }
}

/// <summary>Thrown for invalid input such as NaN, Infinity, or an out-of-range precision. Maps to HTTP 400.</summary>
public sealed class InvalidConversionInputException : ConversionException
{
    /// <summary>Creates the exception with a client-safe explanation.</summary>
    public InvalidConversionInputException(string message)
        : base(message)
    {
    }
}

/// <summary>Thrown when the value maps below absolute zero in the category's base units. Maps to HTTP 400.</summary>
public sealed class BelowAbsoluteZeroException : ConversionException
{
    /// <summary>Creates the exception for a base value below the category minimum.</summary>
    public BelowAbsoluteZeroException(string categoryName, double minimumBaseValue)
        : base($"Value is below absolute zero: category '{categoryName}' requires base values >= {minimumBaseValue}.")
    {
    }
}

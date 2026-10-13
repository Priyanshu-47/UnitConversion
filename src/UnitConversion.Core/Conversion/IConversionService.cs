namespace UnitConversion.Core.Conversion;

/// <summary>
/// Converts numeric values between units. Implementations resolve units through
/// an <c>IUnitCatalog</c>, verify both units share a category, convert via the
/// category base unit, and optionally round the result.
/// </summary>
public interface IConversionService
{
    /// <summary>
    /// Converts <paramref name="value"/> from <paramref name="from"/> to <paramref name="to"/>.
    /// </summary>
    /// <param name="value">Numeric value to convert; must be finite.</param>
    /// <param name="from">Source unit symbol or alias (case-insensitive).</param>
    /// <param name="to">Target unit symbol or alias (case-insensitive).</param>
    /// <param name="precision">Optional rounding to 0-15 decimal places; <see langword="null"/> means no rounding.</param>
    /// <exception cref="Exceptions.UnknownUnitException">A unit token is unknown.</exception>
    /// <exception cref="Exceptions.CategoryMismatchException">Units span different categories.</exception>
    /// <exception cref="Exceptions.InvalidConversionInputException">Value is non-finite or precision is out of range.</exception>
    /// <exception cref="Exceptions.BelowAbsoluteZeroException">Value is below absolute zero for the category.</exception>
    ConversionResult Convert(double value, string from, string to, int? precision = null);
}

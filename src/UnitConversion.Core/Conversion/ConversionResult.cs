namespace UnitConversion.Core.Conversion;

/// <summary>Outcome of a successful conversion. Symbols are canonical catalog symbols.</summary>
public sealed record ConversionResult(
    string Category,
    double Value,
    string From,
    string To,
    double Result);

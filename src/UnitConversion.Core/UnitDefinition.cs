namespace UnitConversion.Core;

/// <summary>
/// A single unit of measurement, defined by an affine mapping to its category's
/// base unit: <c>base = value * Factor + Offset</c>. Converting between any two
/// units of a category goes through the base unit, so N units need only N
/// definitions instead of N*N pairs, and affine units (temperature) work without
/// special cases.
/// </summary>
public sealed class UnitDefinition
{
    /// <summary>Primary symbol used for lookups (e.g. <c>m</c>, <c>km</c>, <c>N·m</c>).</summary>
    public required string Symbol { get; init; }

    /// <summary>Human-readable name (e.g. <c>Kilometer</c>).</summary>
    public required string Name { get; init; }

    /// <summary>Alternative names accepted during lookup (e.g. <c>kilometer</c>, <c>kilometre</c>).</summary>
    public IReadOnlyList<string> Aliases { get; init; } = Array.Empty<string>();

    /// <summary>Multiplier of the affine mapping to the base unit.</summary>
    public double Factor { get; init; } = 1.0;

    /// <summary>Additive offset of the affine mapping to the base unit (0 for multiplicative units).</summary>
    public double Offset { get; init; }

    /// <summary>Converts a value expressed in this unit to the category base unit.</summary>
    public double ToBase(double value) => (value * Factor) + Offset;

    /// <summary>Converts a value expressed in the category base unit to this unit.</summary>
    public double FromBase(double baseValue) => (baseValue - Offset) / Factor;
}

namespace UnitConversion.Core;

/// <summary>
/// A group of units sharing one base unit (e.g. length with base unit meter).
/// The first entry of <see cref="Units"/> is the base unit by convention.
/// </summary>
public sealed class UnitCategory
{
    /// <summary>Stable, URL-friendly identifier (e.g. <c>length</c>, <c>flow-rate</c>).</summary>
    public required string Id { get; init; }

    /// <summary>Display name (e.g. <c>Length</c>).</summary>
    public required string Name { get; init; }

    /// <summary>All units of the category; the first element is the base unit.</summary>
    public required IReadOnlyList<UnitDefinition> Units { get; init; }

    /// <summary>
    /// Optional lower bound expressed in base units. Temperature sets this to
    /// absolute zero (0 kelvin); <see langword="null"/> means unbounded.
    /// </summary>
    public double? MinimumBaseValue { get; init; }
}

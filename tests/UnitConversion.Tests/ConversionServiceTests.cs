using UnitConversion.Core.Catalog;
using UnitConversion.Core.Conversion;
using UnitConversion.Core.Exceptions;

namespace UnitConversion.Tests;

/// <summary>Unit tests for the conversion pipeline: known values, round-trips, errors, aliases.</summary>
public sealed class ConversionServiceTests
{
    private static readonly ConversionService Service = new(new InMemoryUnitCatalog());

    [Theory]
    [InlineData(100, "C", "F", 212)]
    [InlineData(0, "C", "K", 273.15)]
    [InlineData(-273.15, "C", "K", 0)]
    [InlineData(-40, "C", "F", -40)]
    [InlineData(32, "F", "C", 0)]
    [InlineData(1, "km", "m", 1000)]
    [InlineData(1, "mi", "m", 1609.344)]
    [InlineData(1, "atm", "Pa", 101325)]
    [InlineData(1, "ft", "in", 12)]
    public void Converts_KnownValues(double value, string from, string to, double expected)
    {
        var result = Service.Convert(value, from, to);
        Assert.Equal(expected, result.Result, 9);
    }

    [Theory]
    [InlineData(1, "kg", "lb", 2.20462, 1e-5)]
    [InlineData(1, "bar", "psi", 14.5038, 1e-4)]
    [InlineData(1, "lb", "kg", 0.45359237, 1e-12)]
    public void Converts_KnownValuesWithTolerance(double value, string from, string to, double expected, double tolerance)
    {
        var result = Service.Convert(value, from, to);
        Assert.True(
            Math.Abs(result.Result - expected) <= tolerance,
            $"Expected {expected} +/- {tolerance} but got {result.Result}.");
    }

    [Fact]
    public void RoundTrip_EveryUnitPairInEveryCategory_ReturnsOriginalValue()
    {
        var catalog = new InMemoryUnitCatalog();

        foreach (var category in catalog.GetCategories())
        {
            foreach (var source in category.Units)
            {
                foreach (var target in category.Units)
                {
                    var forward = Service.Convert(1, source.Symbol, target.Symbol);
                    var back = Service.Convert(forward.Result, target.Symbol, source.Symbol);
                    var error = Math.Abs(back.Result - 1);

                    Assert.True(
                        error <= 1e-9,
                        $"Round trip failed in '{category.Id}': {source.Symbol} -> {target.Symbol} -> {back.Result} (error {error}).");
                }
            }
        }
    }

    [Fact]
    public void UnknownUnit_Throws()
    {
        var ex = Assert.Throws<UnknownUnitException>(() => Service.Convert(1, "xyzzy", "m"));
        Assert.Contains("xyzzy", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void CrossCategory_ThrowsWithBothCategoriesInMessage()
    {
        var ex = Assert.Throws<CategoryMismatchException>(() => Service.Convert(1, "kg", "mile"));
        Assert.Contains("Mass", ex.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Length", ex.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("Mass", ex.FromCategory);
        Assert.Equal("Length", ex.ToCategory);
    }

    [Theory]
    [InlineData(-274, "C", "F")]
    [InlineData(-500, "K", "C")]
    [InlineData(-460, "F", "C")]
    public void BelowAbsoluteZero_Throws(double value, string from, string to) =>
        Assert.Throws<BelowAbsoluteZeroException>(() => Service.Convert(value, from, to));

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void NonFiniteValue_Throws(double value) =>
        Assert.Throws<InvalidConversionInputException>(() => Service.Convert(value, "m", "km"));

    [Theory]
    [InlineData(-1)]
    [InlineData(16)]
    public void PrecisionOutOfRange_Throws(int precision) =>
        Assert.Throws<InvalidConversionInputException>(() => Service.Convert(1, "m", "km", precision));

    [Fact]
    public void Precision_RoundsResult()
    {
        var result = Service.Convert(1, "m", "mi", 4);
        Assert.Equal(0.0006, result.Result, 4);

        var rounded = Service.Convert(100, "C", "F", 0);
        Assert.Equal(212, rounded.Result, 0);
    }

    [Theory]
    [InlineData("KILOMETERS", "METRES", 1000)]
    [InlineData("Metre", "Centimetres", 100)]
    [InlineData("inch", "MILLIMETRES", 25.4)]
    public void AliasesAndCasing_AreAccepted(string from, string to, double expected)
    {
        var result = Service.Convert(1, from, to);
        Assert.Equal(expected, result.Result, 9);
    }

    [Fact]
    public void Result_ReportsCanonicalSymbolsAndCategory()
    {
        var result = Service.Convert(100, "celsius", "fahrenheit");

        Assert.Equal("temperature", result.Category, StringComparer.OrdinalIgnoreCase);
        Assert.Equal("C", result.From, StringComparer.Ordinal);
        Assert.Equal("F", result.To, StringComparer.Ordinal);
        Assert.Equal(100, result.Value);
        Assert.Equal(212, result.Result, 9);
    }
}

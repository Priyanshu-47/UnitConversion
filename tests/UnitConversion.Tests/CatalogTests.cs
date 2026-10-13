using UnitConversion.Core.Catalog;

namespace UnitConversion.Tests;

/// <summary>Unit tests for catalog lookups.</summary>
public sealed class CatalogTests
{
    private readonly InMemoryUnitCatalog _catalog = new();

    [Fact]
    public void GetCategories_ReturnsAllRequiredCategories()
    {
        var ids = _catalog.GetCategories().Select(c => c.Id).ToArray();

        Assert.Equal(10, ids.Length);
        Assert.Contains("length", ids);
        Assert.Contains("temperature", ids);
        Assert.Contains("mass", ids);
        Assert.Contains("volume", ids);
        Assert.Contains("speed", ids);
        Assert.Contains("pressure", ids);
        Assert.Contains("flow-rate", ids);
        Assert.Contains("power", ids);
        Assert.Contains("torque", ids);
        Assert.Contains("time", ids);
    }

    [Theory]
    [InlineData("m", "length")]
    [InlineData("METRE", "length")]
    [InlineData("K", "temperature")]
    [InlineData("lb", "mass")]
    [InlineData("gpm", "flow-rate")]
    [InlineData("N·m", "torque")]
    public void Resolve_FindsSymbolsAndAliasesCaseInsensitively(string token, string expectedCategoryId)
    {
        var resolved = _catalog.Resolve(token);

        Assert.NotNull(resolved);
        Assert.Equal(expectedCategoryId, resolved!.Category.Id);
    }

    [Fact]
    public void Resolve_UnknownToken_ReturnsNull() =>
        Assert.Null(_catalog.Resolve("parsec-per-fortnight"));

    [Fact]
    public void GetUnits_UnknownCategory_ReturnsNull() =>
        Assert.Null(_catalog.GetUnits("does-not-exist"));

    [Fact]
    public void FindCategory_IsCaseInsensitive()
    {
        var category = _catalog.FindCategory("LENGTH");

        Assert.NotNull(category);
        Assert.Equal("length", category!.Id);
        Assert.NotEmpty(category.Units);
    }
}

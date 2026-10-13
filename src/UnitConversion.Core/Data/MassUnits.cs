namespace UnitConversion.Core.Data;

/// <summary>Mass units. Base unit: kilogram (kg).</summary>
internal static class MassUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "mass",
        Name = "Mass",
        Units = new UnitDefinition[]
        {
            new() { Name = "Kilogram", Symbol = "kg", Aliases = new[] { "kilogram", "kilograms", "kilogramme" } },
            new() { Name = "Gram", Symbol = "g", Aliases = new[] { "gram", "grams" }, Factor = 0.001 },
            new() { Name = "Milligram", Symbol = "mg", Aliases = new[] { "milligram", "milligrams" }, Factor = 0.000_001 },
            new() { Name = "Metric Ton", Symbol = "t", Aliases = new[] { "tonne", "tonnes", "metric ton", "metric tons" }, Factor = 1_000 },
            new() { Name = "Pound", Symbol = "lb", Aliases = new[] { "pound", "pounds" }, Factor = 0.453_592_37 },
            new() { Name = "Ounce", Symbol = "oz", Aliases = new[] { "ounce", "ounces" }, Factor = 0.028_349_523_125 },
        },
    };
}

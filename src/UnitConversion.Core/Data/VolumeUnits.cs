namespace UnitConversion.Core.Data;

/// <summary>Volume units. Base unit: cubic meter (m³). Liquid measures use US definitions.</summary>
internal static class VolumeUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "volume",
        Name = "Volume",
        Units = new UnitDefinition[]
        {
            new() { Name = "Cubic Meter", Symbol = "m3", Aliases = new[] { "m³", "cubic meter", "cubic metre", "cubic meters", "cubic metres" } },
            new() { Name = "Liter", Symbol = "L", Aliases = new[] { "liter", "litre", "liters", "litres" }, Factor = 0.001 },
            new() { Name = "Milliliter", Symbol = "mL", Aliases = new[] { "milliliter", "millilitre", "milliliters", "millilitres" }, Factor = 0.000_001 },
            new() { Name = "Gallon (US)", Symbol = "gal", Aliases = new[] { "gallon", "gallons" }, Factor = 0.003_785_411_784 },
            new() { Name = "Quart (US)", Symbol = "qt", Aliases = new[] { "quart", "quarts" }, Factor = 0.000_946_352_946 },
            new() { Name = "Pint (US)", Symbol = "pt", Aliases = new[] { "pint", "pints" }, Factor = 0.000_473_176_473 },
            new() { Name = "Fluid Ounce (US)", Symbol = "floz", Aliases = new[] { "fl oz", "fl-oz", "fluid ounce", "fluid ounces" }, Factor = 0.000_029_573_529_562_5 },
            new() { Name = "Cubic Inch", Symbol = "in3", Aliases = new[] { "in³", "cubic inch", "cubic inches" }, Factor = 0.000_016_387_064 },
            new() { Name = "Cubic Foot", Symbol = "ft3", Aliases = new[] { "ft³", "cubic foot", "cubic feet" }, Factor = 0.028_316_846_592 },
        },
    };
}

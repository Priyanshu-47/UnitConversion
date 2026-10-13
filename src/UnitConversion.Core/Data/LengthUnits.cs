namespace UnitConversion.Core.Data;

/// <summary>Length units. Base unit: meter (m).</summary>
internal static class LengthUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "length",
        Name = "Length",
        Units = new UnitDefinition[]
        {
            new() { Name = "Meter", Symbol = "m", Aliases = new[] { "meter", "metre", "meters", "metres" } },
            new() { Name = "Kilometer", Symbol = "km", Aliases = new[] { "kilometer", "kilometre", "kilometers", "kilometres" }, Factor = 1_000 },
            new() { Name = "Centimeter", Symbol = "cm", Aliases = new[] { "centimeter", "centimetre", "centimeters", "centimetres" }, Factor = 0.01 },
            new() { Name = "Millimeter", Symbol = "mm", Aliases = new[] { "millimeter", "millimetre", "millimeters", "millimetres" }, Factor = 0.001 },
            new() { Name = "Mile", Symbol = "mi", Aliases = new[] { "mile", "miles" }, Factor = 1_609.344 },
            new() { Name = "Yard", Symbol = "yd", Aliases = new[] { "yard", "yards" }, Factor = 0.9144 },
            new() { Name = "Foot", Symbol = "ft", Aliases = new[] { "foot", "feet" }, Factor = 0.3048 },
            new() { Name = "Inch", Symbol = "in", Aliases = new[] { "inch", "inches" }, Factor = 0.0254 },
            new() { Name = "Nautical Mile", Symbol = "nmi", Aliases = new[] { "nautical mile", "nautical miles" }, Factor = 1_852 },
        },
    };
}

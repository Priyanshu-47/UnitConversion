namespace UnitConversion.Core.Data;

/// <summary>Pressure units. Base unit: pascal (Pa).</summary>
internal static class PressureUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "pressure",
        Name = "Pressure",
        Units = new UnitDefinition[]
        {
            new() { Name = "Pascal", Symbol = "Pa", Aliases = new[] { "pascal", "pascals" } },
            new() { Name = "Kilopascal", Symbol = "kPa", Aliases = new[] { "kilopascal", "kilopascals" }, Factor = 1_000 },
            new() { Name = "Megapascal", Symbol = "MPa", Aliases = new[] { "megapascal", "megapascals" }, Factor = 1_000_000 },
            new() { Name = "Bar", Symbol = "bar", Aliases = new[] { "bars" }, Factor = 100_000 },
            new() { Name = "Millibar", Symbol = "mbar", Aliases = new[] { "millibar", "millibars", "mb" }, Factor = 100 },
            new() { Name = "Pound per Square Inch", Symbol = "psi", Aliases = new[] { "pound per square inch", "lbf/in2" }, Factor = 6_894.757_293_168_361 },
            new() { Name = "Atmosphere", Symbol = "atm", Aliases = new[] { "atmosphere", "atmospheres" }, Factor = 101_325 },
            new() { Name = "Torr", Symbol = "torr", Factor = 101_325d / 760d },
        },
    };
}

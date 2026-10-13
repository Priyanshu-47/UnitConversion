namespace UnitConversion.Core.Data;

/// <summary>
/// Temperature units. Base unit: kelvin (K). Celsius and Fahrenheit are affine
/// (offset != 0), which the base-unit model handles without special cases.
/// Fahrenheit: K = F * 5/9 + 255.372222...
/// </summary>
internal static class TemperatureUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "temperature",
        Name = "Temperature",
        MinimumBaseValue = 0, // absolute zero, expressed in kelvin
        Units = new UnitDefinition[]
        {
            new() { Name = "Kelvin", Symbol = "K", Aliases = new[] { "kelvin", "kelvins" } },
            new() { Name = "Celsius", Symbol = "C", Aliases = new[] { "celsius", "centigrade", "°C" }, Factor = 1, Offset = 273.15 },
            new() { Name = "Fahrenheit", Symbol = "F", Aliases = new[] { "fahrenheit", "°F" }, Factor = 5d / 9d, Offset = 273.15d - (32d * 5d / 9d) },
        },
    };
}

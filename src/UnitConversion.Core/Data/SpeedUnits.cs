namespace UnitConversion.Core.Data;

/// <summary>Speed units. Base unit: meter per second (m/s).</summary>
internal static class SpeedUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "speed",
        Name = "Speed",
        Units = new UnitDefinition[]
        {
            new() { Name = "Meter per Second", Symbol = "m/s", Aliases = new[] { "mps", "meter per second", "metre per second", "meters per second", "metres per second" } },
            new() { Name = "Kilometer per Hour", Symbol = "km/h", Aliases = new[] { "kph", "kmh", "kilometer per hour", "kilometre per hour", "kilometers per hour" }, Factor = 1d / 3.6d },
            new() { Name = "Mile per Hour", Symbol = "mph", Aliases = new[] { "mile per hour", "miles per hour" }, Factor = 0.447_04 },
            new() { Name = "Knot", Symbol = "kn", Aliases = new[] { "knot", "knots" }, Factor = 1_852d / 3_600d },
            new() { Name = "Foot per Second", Symbol = "ft/s", Aliases = new[] { "fps", "foot per second", "feet per second" }, Factor = 0.3048 },
        },
    };
}

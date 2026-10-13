namespace UnitConversion.Core.Data;

/// <summary>Flow rate units. Base unit: cubic meter per second (m³/s).</summary>
internal static class FlowRateUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "flow-rate",
        Name = "Flow Rate",
        Units = new UnitDefinition[]
        {
            new() { Name = "Cubic Meter per Second", Symbol = "m3/s", Aliases = new[] { "m³/s", "cubic meter per second", "cubic metre per second" } },
            new() { Name = "Liter per Second", Symbol = "L/s", Aliases = new[] { "liter per second", "litre per second" }, Factor = 0.001 },
            new() { Name = "Liter per Minute", Symbol = "L/min", Aliases = new[] { "liter per minute", "litre per minute" }, Factor = 0.001 / 60 },
            new() { Name = "Cubic Meter per Hour", Symbol = "m3/h", Aliases = new[] { "m³/h", "cubic meter per hour", "cubic metre per hour" }, Factor = 1d / 3_600d },
            new() { Name = "Gallon per Minute (US)", Symbol = "gpm", Aliases = new[] { "gallon per minute", "gal/min" }, Factor = 0.003_785_411_784 / 60 },
        },
    };
}

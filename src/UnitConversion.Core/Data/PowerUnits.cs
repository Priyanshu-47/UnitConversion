namespace UnitConversion.Core.Data;

/// <summary>Power units. Base unit: watt (W).</summary>
internal static class PowerUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "power",
        Name = "Power",
        Units = new UnitDefinition[]
        {
            new() { Name = "Watt", Symbol = "W", Aliases = new[] { "watt", "watts" } },
            new() { Name = "Kilowatt", Symbol = "kW", Aliases = new[] { "kilowatt", "kilowatts" }, Factor = 1_000 },
            new() { Name = "Megawatt", Symbol = "MW", Aliases = new[] { "megawatt", "megawatts" }, Factor = 1_000_000 },
            new() { Name = "Mechanical Horsepower", Symbol = "hp", Aliases = new[] { "horsepower" }, Factor = 745.699_871_582_270_2 },
        },
    };
}

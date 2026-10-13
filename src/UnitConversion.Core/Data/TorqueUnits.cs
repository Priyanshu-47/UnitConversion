namespace UnitConversion.Core.Data;

/// <summary>Torque units. Base unit: newton meter (N·m).</summary>
internal static class TorqueUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "torque",
        Name = "Torque",
        Units = new UnitDefinition[]
        {
            new() { Name = "Newton Meter", Symbol = "N·m", Aliases = new[] { "Nm", "N*m", "N-m", "newton meter", "newton metre", "newton meters" } },
            new() { Name = "Pound-Force Foot", Symbol = "lbf·ft", Aliases = new[] { "lb-ft", "lbft", "lbf-ft", "pound-foot", "pound feet" }, Factor = 1.355_817_948_331_400_4 },
            new() { Name = "Pound-Force Inch", Symbol = "lbf·in", Aliases = new[] { "lb-in", "lbin", "lbf-in", "pound-inch" }, Factor = 0.112_984_829_027_616_7 },
            new() { Name = "Kilogram-Force Meter", Symbol = "kgf·m", Aliases = new[] { "kgf-m", "kgfm" }, Factor = 9.806_65 },
        },
    };
}

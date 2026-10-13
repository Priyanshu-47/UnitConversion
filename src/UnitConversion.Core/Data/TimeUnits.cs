namespace UnitConversion.Core.Data;

/// <summary>Time units. Base unit: second (s). One year = 365.25 days (Julian year).</summary>
internal static class TimeUnits
{
    internal static UnitCategory Create() => new()
    {
        Id = "time",
        Name = "Time",
        Units = new UnitDefinition[]
        {
            new() { Name = "Second", Symbol = "s", Aliases = new[] { "sec", "secs", "second", "seconds" } },
            new() { Name = "Millisecond", Symbol = "ms", Aliases = new[] { "millisecond", "milliseconds" }, Factor = 0.001 },
            new() { Name = "Minute", Symbol = "min", Aliases = new[] { "minute", "minutes" }, Factor = 60 },
            new() { Name = "Hour", Symbol = "h", Aliases = new[] { "hr", "hrs", "hour", "hours" }, Factor = 3_600 },
            new() { Name = "Day", Symbol = "d", Aliases = new[] { "day", "days" }, Factor = 86_400 },
            new() { Name = "Week", Symbol = "wk", Aliases = new[] { "week", "weeks" }, Factor = 604_800 },
            new() { Name = "Year", Symbol = "yr", Aliases = new[] { "year", "years" }, Factor = 31_557_600 },
        },
    };
}

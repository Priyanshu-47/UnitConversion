namespace UnitConversion.Api.Contracts;

/// <summary>Successful conversion payload.</summary>
public sealed record ConvertResponse(
    string Category,
    double Value,
    string From,
    string To,
    double Result);

/// <summary>Category summary.</summary>
public sealed record CategoryResponse(string Id, string Name);

/// <summary>A unit with its display name, symbol, and accepted aliases.</summary>
public sealed record UnitResponse(string Name, string Symbol, IReadOnlyList<string> Aliases);

/// <summary>Health probe payload.</summary>
public sealed record HealthResponse(string Status);

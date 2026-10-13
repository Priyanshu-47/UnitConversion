# UnitConversion

A small ASP.NET Core Web API that converts numeric values between units of measurement.
Categories: length, temperature, mass, volume, speed, pressure, flow rate, power, torque, time.
Hardcoded unit tables behind an interface, RFC 7807 errors, OpenAPI + interactive docs.

## Run locally

**Prerequisites:** .NET SDK 10 (see `global.json`).

```bash
dotnet restore
dotnet run --project src/UnitConversion.Api
```

- API: `http://localhost:5080`
- Interactive docs (Development): `http://localhost:5080/swagger/` (the root redirects there)
- OpenAPI document: `http://localhost:5080/openapi/v1.json`
- Tests: `dotnet test`

**Docker**

```bash
docker build -t unitconversion .
docker run -p 8080:8080 unitconversion
# http://localhost:8080/api/v1/convert?value=100&from=C&to=F
```

## Examples

```console
$ curl -s "http://localhost:5080/api/v1/convert?value=100&from=C&to=F"
{"category":"temperature","value":100,"from":"C","to":"F","result":211.99999999999997}

$ curl -s "http://localhost:5080/api/v1/convert?value=100&from=C&to=F&precision=2"
{"category":"temperature","value":100,"from":"C","to":"F","result":212}

$ curl -s "http://localhost:5080/api/v1/convert?value=1&from=km&to=mi"
{"category":"length","value":1,"from":"km","to":"mi","result":0.621371192237334}

$ curl -s "http://localhost:5080/api/v1/convert?value=1&from=kg&to=m"
{"type":"https://httpstatuses.com/422","title":"Incompatible categories","status":422,"detail":"Cannot convert between categories 'Mass' and 'Length': both units must belong to the same category.","instance":"/api/v1/convert","traceId":"..."}

$ curl -s "http://localhost:5080/api/v1/convert?value=1&from=xyzzy&to=m"
{"type":"https://httpstatuses.com/400","title":"Unknown unit","status":400,"detail":"Unknown unit 'xyzzy'. List available units via GET /api/v1/categories/{category}/units.","instance":"/api/v1/convert","traceId":"..."}

$ curl -s "http://localhost:5080/api/v1/convert?value=-500&from=K&to=C"
{"type":"https://httpstatuses.com/400","title":"Value below absolute zero","status":400,"detail":"Value is below absolute zero: category 'Temperature' requires base values >= 0.","instance":"/api/v1/convert","traceId":"..."}
```

## API reference

| Method | Path | Description | Success | Errors |
|---|---|---|---|---|
| GET | `/api/v1/convert?value=&from=&to=&precision=` | Convert a value between two units of the same category. `precision` (0-15) optionally rounds; omitted means no rounding. | `200` `{ category, value, from, to, result }` | `400` unknown unit / non-finite value / bad precision / below absolute zero / malformed query, `422` categories differ |
| GET | `/api/v1/categories` | List all categories | `200` `[{ id, name }]` | – |
| GET | `/api/v1/categories/{category}/units` | List units of a category (`name`, `symbol`, `aliases`) | `200` | `404` unknown category |
| GET | `/health` | Liveness probe | `200` `{ status: "Healthy" }` | – |
| GET | `/openapi/v1.json` | OpenAPI document | `200` | – |
| GET | `/swagger/` | Scalar API reference UI (Development only) | `200` | – |

Unit lookup is case-insensitive and accepts symbols and aliases (`m`, `meter`, `metre`, `meters`).

## Solution layout

```
UnitConversion.sln
src/UnitConversion.Core/   domain: UnitDefinition, UnitCategory, IUnitCatalog, IConversionService, per-category data (no ASP.NET dependency)
src/UnitConversion.Api/    endpoints, DI, ProblemDetails handling, OpenAPI
tests/UnitConversion.Tests/ xUnit unit + integration tests (WebApplicationFactory)
```

## Design decisions & trade-offs

**Base-unit model.** Every unit defines `ToBase(x) = x * Factor + Offset` against its category's
base unit (kelvin for temperature, meter for length, …); conversions go value → base → target.
N units need N definitions instead of N×N pairs, so the design scales to hundreds of units with
no per-pair logic. Affine units (Celsius/Fahrenheit offsets) fall out of the same formula — there
is no temperature special case anywhere in the code. The category can declare a minimum base
value (temperature: 0 K) which is validated generically.

**Catalog behind `IUnitCatalog`.** Endpoints and the conversion service depend on an interface,
not on the static tables. A database, JSON file, or admin UI can replace `InMemoryUnitCatalog`
without touching the API layer. Duplicate symbols/aliases fail fast at construction, so data
mistakes surface at startup rather than as wrong lookups in production.

**`double` vs `decimal`.** `double` is used: ±1.8×10³⁰⁸ range covers every physical scale,
IEEE arithmetic is fast, and unit factors are inherently approximate real numbers. `decimal`
would give exact base-10 arithmetic (relevant for currency, not physics) at ~10× lower
throughput and a much narrower range (±7.9×10²⁸). The cost of `double` is binary floating-point
noise — visible above: `100 C → F` returns `211.99999999999997` (1 ulp from 212). Clients that
need clean output pass `precision`, and tests compare with tolerances rather than exact equality.

**Static in-memory data vs persistence.** Unit tables are static code: zero operational
dependencies, versioned with the code, no cache-invalidation problem. The trade-off is that
changing a factor requires a redeploy. When units become user-managed or must change at
runtime, add a persistent `IUnitCatalog` implementation (DB/JSON) and register it in DI —
nothing else changes.

**Alias normalization.** Symbols and aliases are indexed in one `OrdinalIgnoreCase` dictionary,
trimmed at build time. Lookups are case-insensitive (`KILOMETERS`, `kilometres`, `km` all work),
and responses always echo the canonical symbol so callers see stable output.

**Errors as RFC 7807 ProblemDetails.** `AddProblemDetails` + a global `IExceptionHandler` map
domain exceptions to `400`/`422` and everything else to a generic `500` (logged server-side,
never returned). Status-code pages cover unmatched routes/methods. Responses never contain
stack traces or internal details.

**Intentionally left out** (and how to add each):

- **Auth** – add JWT bearer authentication (`AddAuthentication`) and `[RequireAuthorization]`/policies per endpoint.
- **Caching** – cache successful conversions with output caching keyed on `(from, to, precision)`; unit tables are already immutable.
- **Persistence** – implement `IUnitCatalog` over a database and swap the DI registration.
- **Rate limiting** – enable ASP.NET Core's `AddRateLimiter` middleware with a per-IP policy on `/api/v1`.
- **Localization** – move exception messages to resource files and return culture-aware titles/details.

## Adding a new unit

Add one entry to the category's data file — factors are relative to the category base unit:

```csharp
// src/UnitConversion.Core/Data/LengthUnits.cs
new() { Name = "Furlong", Symbol = "fur",
        Aliases = new[] { "furlongs" }, Factor = 201.168 },
```

For a new *category*, copy any file in `Data/` (set `Id`, `Name`, base unit first, optionally
`MinimumBaseValue`) and add it to `InMemoryUnitCatalog.CreateDefaultCategories()`.

using UnitConversion.Api.Contracts;
using UnitConversion.Api.Endpoints;
using UnitConversion.Api.ErrorHandling;
using UnitConversion.Core.Catalog;
using UnitConversion.Core.Conversion;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// RFC 7807 problem details for every error response.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

// OpenAPI document (served at /openapi/v1.json) plus an interactive UI in Development.
builder.Services.AddOpenApi("v1");

// Catalog and conversion engine are interface-backed singletons; swapping the
// in-memory catalog for a database/JSON source does not touch this file's endpoints.
builder.Services.AddSingleton<IUnitCatalog, InMemoryUnitCatalog>();
builder.Services.AddSingleton<IConversionService, ConversionService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseProblemDetailsStatusCodes();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    // Interactive API reference at /swagger; document JSON at /openapi/v1.json.
    app.MapScalarApiReference("/swagger", _ => { });
    app.MapGet("/", () => Results.Redirect("/swagger"))
        .WithName("ApiReferenceRedirect")
        .ExcludeFromDescription();
}

app.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy")))
    .WithName("Health")
    .WithSummary("Liveness probe; returns 200 with { status: 'Healthy' } when the service is running.")
    .Produces<HealthResponse>();

var api = app.MapGroup("/api/v1");
api.MapConversionEndpoints();
api.MapCatalogEndpoints();

app.Run();

// Exposes the implicit Program class to the integration test project.
public partial class Program;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversion.Api.Contracts;

namespace UnitConversion.Tests.Integration;

/// <summary>End-to-end tests through the HTTP pipeline, including error shape.</summary>
public sealed class ConvertApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConvertApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Convert_100CelsiusToFahrenheit_Returns212()
    {
        var response = await _client.GetAsync("/api/v1/convert?value=100&from=C&to=F");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ConvertResponse>();
        Assert.NotNull(body);
        Assert.Equal("temperature", body!.Category);
        Assert.Equal(100, body.Value);
        Assert.Equal("C", body.From);
        Assert.Equal("F", body.To);
        Assert.Equal(212, body.Result, 9);
    }

    [Fact]
    public async Task Convert_UnknownUnit_ReturnsRfc7807ProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/convert?value=1&from=xyzzy&to=m");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var raw = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("stack", raw, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("   at ", raw, StringComparison.Ordinal);

        var problem = await response.Content.ReadFromJsonAsync<ProblemBody>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem!.Status);
        Assert.False(string.IsNullOrWhiteSpace(problem.Title));
        Assert.False(string.IsNullOrWhiteSpace(problem.Detail));
        Assert.Contains("xyzzy", problem.Detail, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Convert_CrossCategory_Returns422ProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/convert?value=1&from=kg&to=m");

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ProblemBody>();
        Assert.NotNull(problem);
        Assert.Equal(422, problem!.Status);
        Assert.Contains("Mass", problem.Detail, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Length", problem.Detail, StringComparison.OrdinalIgnoreCase);
    }

    private sealed record ProblemBody(int? Status, string? Title, string? Detail, string? Type, string? Instance);
}

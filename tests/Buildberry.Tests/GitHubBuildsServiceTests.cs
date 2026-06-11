using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Buildberry.Models;
using Buildberry.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Buildberry.Tests;

class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _response;

    public MockHttpMessageHandler(HttpResponseMessage response) => _response = response;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}

public class GitHubBuildsServiceTests
{
    [Fact]
    public async Task GetBuildsAsync_ReturnsRuns_WhenApiReturnsRuns()
    {
        // Arrange
        var json = @"{ ""workflow_runs"": [ { ""name"": ""CI"", ""conclusion"": ""success"", ""updated_at"": ""2026-06-09T12:00:00Z"" } ] }";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler);
        var logger = new NullLogger<GitHubBuildsService>();
        var service = new GitHubBuildsService(logger, httpClient);

        // Act
        var runs = await service.GetBuildsAsync();

        // Assert
        runs.Should().NotBeNull();
        runs.Should().HaveCountGreaterOrEqualTo(1);
        var first = System.Linq.Enumerable.First(runs);
        first.Workflow.Should().Be("CI");
        first.Status.Should().Be("success");
    }

    [Fact]
    public async Task GetBuildsAsync_ReturnsEmpty_WhenNoRuns()
    {
        // Arrange
        var json = @"{ ""workflow_runs"": [] }";
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler);
        var logger = new NullLogger<GitHubBuildsService>();
        var service = new GitHubBuildsService(logger, httpClient);

        // Act
        var runs = await service.GetBuildsAsync();

        // Assert
        runs.Should().NotBeNull();
        runs.Should().BeEmpty();
    }
}

using System.Net.Http.Headers;
using System.Text.Json;
using Buildberry.Models;
using Microsoft.Extensions.Logging;

namespace Buildberry.Services;

/// <summary>
/// Simple GitHub client that fetches workflow runs and maps them to WorkflowRun.
/// Uses a personal access token from environment variable GITHUB_PAT if available.
/// This implementation is intentionally minimal for MVP and unit testing.
/// </summary>
public class GitHubBuildsService : IBuildsService
{
    private readonly ILogger<GitHubBuildsService> _logger;
    private readonly HttpClient _httpClient;

    public GitHubBuildsService(ILogger<GitHubBuildsService> logger, HttpClient? httpClient = null)
    {
        _logger = logger;
        _httpClient = httpClient ?? new HttpClient();

        var pat = Environment.GetEnvironmentVariable("GITHUB_PAT");
        if (!string.IsNullOrEmpty(pat))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", pat);
        }
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Buildberry/1.0");
    }

    /// <summary>
    /// Fetches latest workflow runs for a small set of example repositories.
    /// For MVP this method demonstrates mapping; later it should accept owner/repo or use authenticated user repos.
    /// </summary>
    public async Task<IEnumerable<WorkflowRun>> GetBuildsAsync()
    {
        _logger.LogInformation("Fetching builds from GitHub");

        // For MVP, use a small hardcoded repo list. This keeps behavior deterministic for tests.
        var repos = new List<(string owner, string repo)>
        {
            ("ChipyCao", "buildberry")
        };

        var results = new List<WorkflowRun>();

        foreach (var (owner, repo) in repos)
        {
            try
            {
                var url = $"https://api.github.com/repos/{owner}/{repo}/actions/runs?per_page=5";
                using var resp = await _httpClient.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GitHub API returned {Status} for {Repo}", resp.StatusCode, repo);
                    continue;
                }
                using var stream = await resp.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                if (doc.RootElement.TryGetProperty("workflow_runs", out var runs))
                {
                    foreach (var run in runs.EnumerateArray())
                    {
                        var workflowName = run.GetProperty("name").GetString() ?? run.GetProperty("workflow_id").GetRawText();
                        var status = (run.TryGetProperty("conclusion", out var c) && c.ValueKind == JsonValueKind.String)
                            ? c.GetString() ?? "unknown"
                            : run.GetProperty("status").GetString() ?? "unknown";

                        var updatedAt = run.GetProperty("updated_at").GetDateTime();

                        results.Add(new WorkflowRun
                        {
                            Repository = repo,
                            Workflow = workflowName,
                            Status = status,
                            UpdatedAt = updatedAt
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching runs for {Owner}/{Repo}", owner, repo);
            }
        }

        return results;
    }
}

using Buildberry.Models;

namespace Buildberry.Services;

/// <summary>
/// Service for managing GitHub Actions workflow builds.
/// Currently returns hardcoded sample data.
/// Future implementation will integrate with the GitHub API.
/// </summary>
public class BuildsService : IBuildsService
{
    private readonly ILogger<BuildsService> _logger;

    public BuildsService(ILogger<BuildsService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of workflow runs.
    /// Currently returns hardcoded sample data for testing.
    /// </summary>
    /// <returns>An enumerable collection of sample WorkflowRun objects.</returns>
    public Task<IEnumerable<WorkflowRun>> GetBuildsAsync()
    {
        _logger.LogInformation("Fetching builds from service");

        var sampleData = new List<WorkflowRun>
        {
            new WorkflowRun
            {
                Repository = "buildberry",
                Workflow = "CI",
                Status = "success",
                UpdatedAt = DateTime.Parse("2026-06-09T12:00:00Z")
            }
        };

        return Task.FromResult<IEnumerable<WorkflowRun>>(sampleData);
    }
}

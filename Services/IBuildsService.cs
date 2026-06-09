using Buildberry.Models;

namespace Buildberry.Services;

/// <summary>
/// Service interface for retrieving and managing GitHub Actions workflow builds.
/// </summary>
public interface IBuildsService
{
    /// <summary>
    /// Retrieves a list of workflow runs asynchronously.
    /// </summary>
    /// <returns>An enumerable collection of WorkflowRun objects.</returns>
    Task<IEnumerable<WorkflowRun>> GetBuildsAsync();
}

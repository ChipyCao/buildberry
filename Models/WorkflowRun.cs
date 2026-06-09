namespace Buildberry.Models;

/// <summary>
/// Represents a GitHub Actions workflow run.
/// </summary>
public class WorkflowRun
{
    /// <summary>
    /// The repository name.
    /// </summary>
    public required string Repository { get; set; }

    /// <summary>
    /// The workflow name.
    /// </summary>
    public required string Workflow { get; set; }

    /// <summary>
    /// The status of the workflow run (e.g., "success", "failure", "pending").
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The timestamp when the workflow run was last updated.
    /// </summary>
    public required DateTime UpdatedAt { get; set; }
}

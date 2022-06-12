using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities;

/// <summary>
/// Orchestrates how an activity should be run.
/// </summary>
public interface IActivityOrchestrator
{
    /// <summary>
    /// Run the orchestrated activity given it's active state.
    /// </summary>
    /// <param name="activeState">The active state of the robot.</param>
    /// <returns><c>True</c> if the activity completed successfully, else <c>false</c>.</returns>
    Task<bool> Run(RobotState activeState);
}
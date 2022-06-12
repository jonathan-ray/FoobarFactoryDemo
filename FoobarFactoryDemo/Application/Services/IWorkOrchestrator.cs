namespace FoobarFactoryDemo.Application.Services;

/// <summary>
/// Orchestrator of the work to be done.
/// </summary>
/// <typeparam name="TWorkerState">Representation of the state of a worker in between activities.</typeparam>
public interface IWorkOrchestrator<TWorkerState>
{
    /// <summary>
    /// Helper method for creating the initial state of a new worker.
    /// </summary>
    /// <param name="workerId">The ID of the worker.</param>
    /// <returns>The worker's initial state.</returns>
    TWorkerState CreateInitialState(int workerId);

    /// <summary>
    /// Runs the orchestration of work.
    /// </summary>
    /// <param name="previousState">State of the worker pre-run.</param>
    /// <returns>State of the worker post-run.</returns>
    Task<TWorkerState> Run(TWorkerState previousState);
}
namespace FoobarFactoryDemo.Presentation;

/// <summary>
/// Service to update the presentation layer on worker-specific topics.
/// </summary>
/// <typeparam name="TWorkerState">Representation of the state of a worker in between activities.</typeparam>
public interface IWorkerPresentationService<in TWorkerState>
{
    /// <summary>
    /// Declare activity of a worker's current state has started.
    /// </summary>
    /// <param name="workerState">The worker's state.</param>
    void ActivityStarted(TWorkerState workerState);

    /// <summary>
    /// Declare activity of a worker's current state has completed successfully.
    /// </summary>
    /// <param name="workerState">The worker's state.</param>
    void ActivityCompleted(TWorkerState workerState);

    /// <summary>
    /// Declare activity of a worker's current state has failed to complete.
    /// </summary>
    /// <param name="workerState">The worker's state.</param>
    void ActivityFailed(TWorkerState workerState);

    /// <summary>
    /// Declare a worker has been added.
    /// </summary>
    /// <param name="totalWorkers">New total worker count.</param>
    void WorkerAdded(int totalWorkers);
}
namespace FoobarFactoryDemo.Application.Services;

/// <summary>
/// Abstraction of a production line processing activities by workers.
/// </summary>
/// <typeparam name="TWorkerState">Representation of the state of a worker in between activities.</typeparam>
public interface IProductionLine<TWorkerState>
{
    /// <summary>
    /// Amount of active workers.
    /// </summary>
    int WorkerCount { get; }

    /// <summary>
    /// Defines whether there are incomplete activities still remaining to finish.
    /// </summary>
    bool HasIncompleteActivities { get; }

    /// <summary>
    /// Initializes and starts the production line.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Adds a worker to the production line.
    /// </summary>
    void AddWorker();

    /// <summary>
    /// Creates a new activity to be completed, given the state of how the worker ended their last.
    /// </summary>
    /// <param name="previousState">The state of the worker's previous activity.</param>
    void CreateNewActivityFromPrevious(TWorkerState previousState);

    /// <summary>
    /// Gets the next activity to complete.
    /// </summary>
    /// <returns>The state of the worker after the activity.</returns>
    Task<TWorkerState> NextCompletedActivity();
}
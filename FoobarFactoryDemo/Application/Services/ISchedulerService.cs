namespace FoobarFactoryDemo.Application.Services;

/// <summary>
/// Scheduler service that given work to do will continuously add more when completed.
/// </summary>
public interface ISchedulerService
{
    /// <summary>
    /// Starts the scheduler service.
    /// </summary>
    Task Start();
}
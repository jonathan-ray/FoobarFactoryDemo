using FoobarFactoryDemo.Application.Services;

namespace FoobarFactoryDemo.Application.Activities.Factories;

/// <summary>
/// A factory for creating the orchestrator for work to be done.
/// </summary>
/// <typeparam name="TWorkerState">Representation of the state of a worker in between activities.</typeparam>
public interface IWorkOrchestratorFactory<TWorkerState>
{
    /// <summary>
    /// Creates the work orchestrator.
    /// </summary>
    /// <param name="productionLine">The production line the work is being completed for.</param>
    /// <returns>The work orchestrator.</returns>
    IWorkOrchestrator<TWorkerState> CreateWork(IProductionLine<TWorkerState> productionLine);
}
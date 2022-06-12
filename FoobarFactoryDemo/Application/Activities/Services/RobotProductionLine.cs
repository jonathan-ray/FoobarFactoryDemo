using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Application.Activities.Services;

public class RobotProductionLine : IProductionLine<RobotState>
{
    private readonly IWorkerPresentationService<RobotState> presentationService;
    private readonly IWorkOrchestrator<RobotState> robotWorkOrchestrator;
    private readonly List<Task<RobotState>> activeActivities;

    public RobotProductionLine(
        IWorkOrchestratorFactory<RobotState> workOrchestratorFactory,
        IOptions<FactorySettings> factorySettings,
        IWorkerPresentationService<RobotState> presentationService)
    {
        if (workOrchestratorFactory == null)
        {
            throw new ArgumentNullException(nameof(workOrchestratorFactory));
        }

        this.robotWorkOrchestrator = workOrchestratorFactory.CreateWork(this);

        if (factorySettings?.Value == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        if (factorySettings.Value.InitialWorkerCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(factorySettings), "You must start with at least one robot.");
        }

        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));

        this.WorkerCount = factorySettings.Value.InitialWorkerCount;
        this.activeActivities = new List<Task<RobotState>>();
    }

    public int WorkerCount { get; private set; }

    public bool HasIncompleteActivities => this.activeActivities.Count > 0;

    public void Initialize()
    {
        this.activeActivities.Clear();

        for (var i = 0; i < this.WorkerCount; i++)
        {
            this.CreateNewActivityFromPrevious(this.robotWorkOrchestrator.CreateInitialState(i));
        }
    }

    public void AddWorker()
    {
        if (this.activeActivities.Count > 0)
        {
            this.CreateNewActivityFromPrevious(this.robotWorkOrchestrator.CreateInitialState(this.WorkerCount));
        }

        this.WorkerCount++;
        this.presentationService.WorkerAdded(this.WorkerCount);
    }

    public void CreateNewActivityFromPrevious(RobotState previousState)
    {
        this.activeActivities.Add(this.robotWorkOrchestrator.Run(previousState));
    }

    public async Task<RobotState> NextCompletedActivity()
    {
        if (this.activeActivities.Count == 0)
        {
            throw new EmptyProductionLineException();
        }

        var completedActivityTask = await Task.WhenAny(this.activeActivities);
        this.activeActivities.Remove(completedActivityTask);
        return await completedActivityTask;
    }
}
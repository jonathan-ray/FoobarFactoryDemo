using FoobarFactoryDemo.Application.Activities.Services;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Application.Activities.Factories;

public class RobotWorkOrchestratorFactory : IWorkOrchestratorFactory<RobotState>
{
    private readonly IActivityPicker activityPicker;
    private readonly IActivityDurationsFactory activityDurationsFactory;
    private readonly IRandomGenerator randomGenerator;
    private readonly IResourceRepository resourceRepository;
    private readonly IWorkerPresentationService<RobotState> presentationService;

    public RobotWorkOrchestratorFactory(
        IActivityPicker activityPicker,
        IActivityDurationsFactory activityDurationsFactory,
        IRandomGenerator randomGenerator,
        IResourceRepository resourceRepository,
        IWorkerPresentationService<RobotState> presentationService)
    {
        this.activityPicker = activityPicker ?? throw new ArgumentNullException(nameof(activityPicker));
        this.activityDurationsFactory = activityDurationsFactory ?? throw new ArgumentNullException(nameof(activityDurationsFactory));
        this.randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
        this.resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));
    }

    public IWorkOrchestrator<RobotState> CreateWork(IProductionLine<RobotState> productionLine)
    {
        return new RobotWorkOrchestrator(
            this.activityPicker,
            new ActivityOrchestrator(
                this.activityDurationsFactory.CreateDurations(), 
                this.randomGenerator,
                this.resourceRepository,
                productionLine), 
            this.presentationService);
    }
}
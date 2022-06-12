using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Application.Services;

public class SchedulerService<TWorkerState> : ISchedulerService
{
    private readonly IAutoShutdownService shutdownService;
    private readonly IProductionLine<TWorkerState> productionLine;
    private readonly IPresentationService presentationService;

    public SchedulerService(
        IAutoShutdownService shutdownService,
        IProductionLine<TWorkerState> productionLine,
        IPresentationService presentationService)
    {
        this.shutdownService = shutdownService ?? throw new ArgumentNullException(nameof(shutdownService));
        this.productionLine = productionLine ?? throw new ArgumentNullException(nameof(productionLine));
        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));

        this.IsActive = false;
    }

    private bool IsActive { get; set; }

    public async Task Start()
    {
        this.IsActive = true;
        this.presentationService.SchedulerStarted();

        this.productionLine.Initialize();

        while (this.productionLine.HasIncompleteActivities)
        {
            var completedState = await this.productionLine.NextCompletedActivity();
            if (this.IsActive)
            {
                if (!this.shutdownService.ShouldShutdown)
                {
                    this.productionLine.CreateNewActivityFromPrevious(completedState);
                }
                else
                {
                    this.presentationService.ShutdownInitiated();
                    this.IsActive = false;
                }
            }
        }

        this.presentationService.SchedulerEnded();
    }
}
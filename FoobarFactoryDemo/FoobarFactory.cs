using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo;

public class FoobarFactory : IFoobarFactory
{
    private readonly ISchedulerService schedulerService;
    private readonly IPresentationService presentationService;

    public FoobarFactory(
        ISchedulerService schedulerService,
        IPresentationService presentationService)
    {
        this.schedulerService = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));
    }

    public async Task Run()
    {
        this.presentationService.WelcomeMessage();
        await this.presentationService.Pause();
        await this.schedulerService.Start();
    }
}
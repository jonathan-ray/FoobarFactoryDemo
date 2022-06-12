using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Application.Activities.Services;

public class RobotWorkOrchestrator : IWorkOrchestrator<RobotState>
{
    private readonly IActivityPicker activityPicker;
    private readonly IActivityOrchestrator activityOrchestrator;
    private readonly IWorkerPresentationService<RobotState> presentationService;

    public RobotWorkOrchestrator(
        IActivityPicker activityPicker,
        IActivityOrchestrator activityOrchestrator,
        IWorkerPresentationService<RobotState> presentationService)
    {
        this.activityPicker = activityPicker ?? throw new ArgumentNullException(nameof(activityPicker));
        this.activityOrchestrator = activityOrchestrator ?? throw new ArgumentNullException(nameof(activityOrchestrator));
        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));
    }

    public RobotState CreateInitialState(int workerId) => new(workerId, ActivityType.Idling);

    public async Task<RobotState> Run(RobotState previousState)
    {
        var currentState = previousState with
        {
            Activity = this.activityPicker.GetNextActivity(previousState.Activity)
        };

        this.presentationService.ActivityStarted(currentState);

        var result = await this.activityOrchestrator.Run(currentState);

        if (result)
        {
            this.presentationService.ActivityCompleted(currentState);
        }
        else
        {
            this.presentationService.ActivityFailed(currentState);
        }

        return currentState;
    }
}
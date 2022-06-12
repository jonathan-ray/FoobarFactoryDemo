using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Application.Activities;

public class ActivityOrchestrator : IActivityOrchestrator
{
    private readonly ActivityDurations activityDurations;

    private readonly IActivity miningFooActivity;
    private readonly IActivity miningBarActivity;
    private readonly IActivity assemblingFoobarActivity;
    private readonly IActivity sellingFoobarActivity;
    private readonly IActivity buyingRobotActivity;
    private readonly IActivity changingActivity;

    public ActivityOrchestrator(
        ActivityDurations activityDurations,
        IRandomGenerator randomGenerator,
        IResourceRepository resourceRepository,
        IProductionLine<RobotState> productionLine)
    {
        if (randomGenerator == null)
        {
            throw new ArgumentNullException(nameof(randomGenerator));
        }

        if (resourceRepository == null)
        {
            throw new ArgumentNullException(nameof(resourceRepository));
        }

        if (productionLine == null)
        {
            throw new ArgumentNullException(nameof(productionLine));
        }

        this.activityDurations = activityDurations ?? throw new ArgumentNullException(nameof(activityDurations));

        this.miningFooActivity = new MiningFooActivity(resourceRepository);
        this.miningBarActivity = new MiningBarActivity(resourceRepository);
        this.assemblingFoobarActivity = new AssemblingFoobarActivity(randomGenerator, resourceRepository);
        this.sellingFoobarActivity = new SellingFoobarActivity(resourceRepository);
        this.buyingRobotActivity = new BuyingRobotActivity(resourceRepository, productionLine);
        this.changingActivity = new ChangingActivity();
    }

    public async Task<bool> Run(RobotState activeState)
    {
        var activity = activeState.Activity switch
        {
            ActivityType.MiningFoo => this.miningFooActivity,
            ActivityType.MiningBar => this.miningBarActivity,
            ActivityType.AssemblingFoobar => this.assemblingFoobarActivity,
            ActivityType.SellingFoobar => this.sellingFoobarActivity,
            ActivityType.BuyingRobot => this.buyingRobotActivity,
            ActivityType.ChangingActivity => this.changingActivity,
            _ => throw new NotSupportedException($"Activity type '{activeState.Activity}' is not supported for running.")
        };

        return await this.Run(activity);
    }

    private async Task<bool> Run(IActivity activity)
    {
        var successfulRun = false;
        var resourceResult = activity.GetRequiredResources();

        // If we have the required resources, complete the operation in the expected duration.
        // Else, there's no need to wait the duration of the activity.
        if (resourceResult.WasSuccessful)
        {
            await Task.Delay(this.activityDurations.GetDuration(activity.Type));
            successfulRun = activity.Run(resourceResult);
        }

        return successfulRun;
    }
}
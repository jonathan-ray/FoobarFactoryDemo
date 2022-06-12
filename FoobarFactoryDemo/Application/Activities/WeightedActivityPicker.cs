using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Application.Activities;

/// <summary>
/// Activity picker based on a weighted automaton.
/// </summary>
public class WeightedActivityPicker : IActivityPicker
{
    private readonly IRandomGenerator randomGenerator;
    private readonly IActivityWeights weights;

    public WeightedActivityPicker(
        IRandomGenerator randomGenerator,
        IActivityWeightsFactory activityWeightsFactory)
    {
        this.randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
        this.weights = (activityWeightsFactory ?? throw new ArgumentNullException(nameof(activityWeightsFactory))).CreateActivityWeights();
    }

    public ActivityType GetNextActivity(ActivityType previousActivity)
    {
        // If idling (i.e. starting), you always need to change activity next:
        if (previousActivity == ActivityType.Idling)
        {
            return ActivityType.ChangingActivity;
        }

        // Get random value to decide where to go next
        var randomValue = this.randomGenerator.NextDouble();

        // If changing activity, choose one of the actionable activities:
        if (previousActivity == ActivityType.ChangingActivity)
        {
            return this.weights.GetActionableActivity(randomValue);
        }

        // If already doing an actionable activity, either continue or change activity:
        return this.weights.GetChangeableActivity(randomValue, previousActivity);
    }
}
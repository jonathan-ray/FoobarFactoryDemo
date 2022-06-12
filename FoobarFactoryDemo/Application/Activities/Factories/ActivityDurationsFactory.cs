using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Application.Activities.Factories;

public class ActivityDurationsFactory : IActivityDurationsFactory
{
    private readonly IRandomGenerator randomGenerator;
    private readonly ActivityDurationSettings activityDurationSettings;

    public ActivityDurationsFactory(
        IRandomGenerator randomGenerator,
        IOptions<ActivityDurationSettings> activityDurationSettings)
    {
        this.randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
        this.activityDurationSettings = activityDurationSettings?.Value ?? throw new ArgumentNullException(nameof(activityDurationSettings));
    }

    public ActivityDurations CreateDurations()
    {
        return new ActivityDurations(
            MiningFoo: this.GenerateDurationValue(this.activityDurationSettings.MiningFooDuration),
            MiningBar: new VariableTimeSpan(
                this.randomGenerator, 
                this.GenerateDurationValue(this.activityDurationSettings.MiningBarDurationRangeStart),
                this.GenerateDurationValue(this.activityDurationSettings.MiningBarDurationRangeEnd)),
            AssemblingFoobar: this.GenerateDurationValue(this.activityDurationSettings.AssemblingFoobarDuration),
            SellingFoobar: this.GenerateDurationValue(this.activityDurationSettings.SellingFoobarDuration),
            BuyingRobot: this.GenerateDurationValue(this.activityDurationSettings.BuyingRobotDuration),
            ChangingActivity: this.GenerateDurationValue(this.activityDurationSettings.ChangingActivityDuration));
    }

    private TimeSpan GenerateDurationValue(double initialMsValue)
    {
        return this.activityDurationSettings.InverseDurationCoefficient * TimeSpan.FromMilliseconds(initialMsValue);
    }
}
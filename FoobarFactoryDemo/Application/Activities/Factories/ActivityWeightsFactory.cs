using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Application.Activities.Factories;

public class ActivityWeightsFactory : IActivityWeightsFactory
{
    private readonly WeightedActivityPickerSettings activityPickerSettings;

    public ActivityWeightsFactory(IOptions<WeightedActivityPickerSettings> activityPickerSettings)
    {
        this.activityPickerSettings = activityPickerSettings?.Value ?? throw new ArgumentNullException(nameof(activityPickerSettings));
    }

    public IActivityWeights CreateActivityWeights()
    {
        if (this.activityPickerSettings.ChangingActivityWeight is <= 0 or >= 1)
        {
            throw new ChangingActivityWeightOutOfRangeException(this.activityPickerSettings.ChangingActivityWeight);
        }

        var actionableActivityWeightTotal = this.activityPickerSettings.MiningFooWeight
            + this.activityPickerSettings.MiningBarWeight
            + this.activityPickerSettings.AssemblingFoobarWeight
            + this.activityPickerSettings.SellingFoobarWeight
            + this.activityPickerSettings.BuyingRobotWeight;

        if (actionableActivityWeightTotal == 0)
        {
            throw new ActionableActivityWeightOutOfRangeException();
        }

        var miningFooWeightRangeEnd = this.activityPickerSettings.MiningFooWeight / actionableActivityWeightTotal;
        var miningBarWeightRangeEnd = this.activityPickerSettings.MiningBarWeight / actionableActivityWeightTotal + miningFooWeightRangeEnd;
        var assemblingFoobarWeightRangeEnd = this.activityPickerSettings.AssemblingFoobarWeight / actionableActivityWeightTotal + miningBarWeightRangeEnd;
        var sellingFoobarWeightRangeEnd = this.activityPickerSettings.SellingFoobarWeight / actionableActivityWeightTotal + assemblingFoobarWeightRangeEnd;

        return new ActivityWeights(
            MiningFoo: new WeightRange(0, miningFooWeightRangeEnd),
            MiningBar: new WeightRange(miningFooWeightRangeEnd, miningBarWeightRangeEnd),
            AssemblingFoobar: new WeightRange(miningBarWeightRangeEnd, assemblingFoobarWeightRangeEnd),
            SellingFoobar: new WeightRange(assemblingFoobarWeightRangeEnd, sellingFoobarWeightRangeEnd),
            BuyingRobot: new WeightRange(sellingFoobarWeightRangeEnd, 1),
            ChangingActivity: new WeightRange(0, this.activityPickerSettings.ChangingActivityWeight));
    }
}
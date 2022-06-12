namespace FoobarFactoryDemo.Models.Domain;

/// <summary>
/// Definition of weights applied to each activity.
/// </summary>
/// <param name="MiningFoo">The weight range for mining Foo.</param>
/// <param name="MiningBar">The weight range for mining Bar.</param>
/// <param name="AssemblingFoobar">The weight range for assembling Foobar.</param>
/// <param name="SellingFoobar">The weight range for selling Foobar.</param>
/// <param name="BuyingRobot">The weight range for buying Robots.</param>
/// <param name="ChangingActivity">The weight range for changing activities.</param>
/// <remarks>Actionable activities are all predetermined divided subsections of the range 0 to 1.</remarks>.
public record ActivityWeights(
    WeightRange MiningFoo,
    WeightRange MiningBar,
    WeightRange AssemblingFoobar,
    WeightRange SellingFoobar,
    WeightRange BuyingRobot,
    WeightRange ChangingActivity) : IActivityWeights
{
    public ActivityType GetActionableActivity(double selectorValue)
    {
        if (this.MiningFoo.Start <= selectorValue && selectorValue < this.MiningFoo.End)
        {
            return ActivityType.MiningFoo;
        }

        if (this.MiningBar.Start <= selectorValue && selectorValue < this.MiningBar.End)
        {
            return ActivityType.MiningBar;
        }

        if (this.AssemblingFoobar.Start <= selectorValue && selectorValue < this.AssemblingFoobar.End)
        {
            return ActivityType.AssemblingFoobar;
        }

        if (this.SellingFoobar.Start <= selectorValue && selectorValue < this.SellingFoobar.End)
        {
            return ActivityType.SellingFoobar;
        }

        if (this.BuyingRobot.Start <= selectorValue && selectorValue < this.BuyingRobot.End)
        {
            return ActivityType.BuyingRobot;
        }

        throw new ArgumentOutOfRangeException(nameof(selectorValue), "Selector value should be between 0 and 1.");
    }

    public ActivityType GetChangeableActivity(double selectorValue, ActivityType previousActivity)
    {
        if (selectorValue is < 0 or >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(selectorValue), "Selector value should be between 0 and 1.");
        }

        return this.ChangingActivity.Start <= selectorValue && selectorValue < this.ChangingActivity.End
            ? ActivityType.ChangingActivity
            : previousActivity;
    }
}
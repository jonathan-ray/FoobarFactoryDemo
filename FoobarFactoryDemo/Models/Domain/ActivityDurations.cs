namespace FoobarFactoryDemo.Models.Domain;

/// <summary>
/// Calculated definitions of how long activities should take.
/// </summary>
/// <param name="MiningFoo">How long it should take to perform the Mining Foo activity.</param>
/// <param name="MiningBar">How long it should take to perform the Mining Bar activity.</param>
/// <param name="AssemblingFoobar">How long it should take to perform the Assembling Foobar activity.</param>
/// <param name="SellingFoobar">How long it should take to perform the Selling Foobar activity.</param>
/// <param name="BuyingRobot">How long it should take to perform the Buying Robot activity.</param>
/// <param name="ChangingActivity">How long it should take to change activity.</param>
public record ActivityDurations(
    TimeSpan MiningFoo, 
    VariableTimeSpan MiningBar,
    TimeSpan AssemblingFoobar,
    TimeSpan SellingFoobar,
    TimeSpan BuyingRobot,
    TimeSpan ChangingActivity)
{
    /// <summary>
    /// Helper method for retrieving a duration given the Activity's type.
    /// </summary>
    /// <param name="activityType">The activity's type.</param>
    /// <returns>The duration it should take.</returns>
    public TimeSpan GetDuration(ActivityType activityType)
    {
        return activityType switch
        {
            ActivityType.MiningFoo => this.MiningFoo,
            ActivityType.MiningBar => this.MiningBar.NextValue,
            ActivityType.AssemblingFoobar => this.AssemblingFoobar,
            ActivityType.SellingFoobar => this.SellingFoobar,
            ActivityType.BuyingRobot => this.BuyingRobot,
            ActivityType.ChangingActivity => this.ChangingActivity,
            _ => throw new NotSupportedException($"Activity type '{activityType}' does not have a supported duration.")
        };
    }
}
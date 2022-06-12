namespace FoobarFactoryDemo.Infrastructure.Settings;

/// <summary>
/// Collection of weight definitions to be used by the weighted activity picker.
/// </summary>
public class WeightedActivityPickerSettings
{
    /// <summary>
    /// The weighting applied to the mining Foo activity, as compared with other actionable activities.
    /// </summary>
    public double MiningFooWeight { get; init; }

    /// <summary>
    /// The weighting applied to the mining Bar activity, as compared with other actionable activities.
    /// </summary>
    public double MiningBarWeight { get; init; }

    /// <summary>
    /// The weighting applied to the assembling Foobar activity, as compared with other actionable activities.
    /// </summary>
    public double AssemblingFoobarWeight { get; init; }

    /// <summary>
    /// The weighting applied to the selling Foobar activity, as compared with other actionable activities.
    /// </summary>
    public double SellingFoobarWeight { get; init; }

    /// <summary>
    /// The weighting applied to the buying robot activity, as compared with other actionable activities.
    /// </summary>
    public double BuyingRobotWeight { get; init; }

    /// <summary>
    /// The weighting for changing activity, when performing an actionable activity.
    /// </summary>
    public double ChangingActivityWeight { get; init; }
}
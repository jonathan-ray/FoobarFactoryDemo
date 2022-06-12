namespace FoobarFactoryDemo.Infrastructure.Settings;

public class ActivityDurationSettings
{
    /// <summary>
    /// A multiplier to apply to durations to linearly speed up/slow down the production line.
    /// In essence, defining how many real-time milliseconds should be in a system-time second.
    /// </summary>
    public double InverseDurationCoefficient { get; init; }

    /// <summary>
    /// Defines how long it should take to mine Foo.
    /// </summary>
    public double MiningFooDuration { get; init; }

    /// <summary>
    /// Defines the starting range for how long it should take to mine Bar.
    /// </summary>
    public double MiningBarDurationRangeStart { get; init; }

    /// <summary>
    /// Defines the ending range for how long it should take to mine Bar.
    /// </summary>
    public double MiningBarDurationRangeEnd { get; init; }

    /// <summary>
    /// Defines how long it should take to assemble a Foobar.
    /// </summary>
    public double AssemblingFoobarDuration { get; init; }

    /// <summary>
    /// Defines how long it should take to sell Foobar.
    /// </summary>
    public double SellingFoobarDuration { get; init; }

    /// <summary>
    /// Defines how long it should take to buy a robot.
    /// </summary>
    public double BuyingRobotDuration { get; init; }

    /// <summary>
    /// Defines how long it should take to change activity.
    /// </summary>
    public double ChangingActivityDuration { get; init; }
}
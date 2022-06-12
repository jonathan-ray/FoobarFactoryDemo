namespace FoobarFactoryDemo.Models.Domain;

/// <summary>
/// Types of actions.
/// </summary>
public enum ActivityType
{
    /// <summary>
    /// Work (not) done when the production line is stopped.
    /// </summary>
    Idling = 0,

    /// <summary>
    /// Work done when mining Foo.
    /// </summary>
    MiningFoo,

    /// <summary>
    /// Work done when mining Bar.
    /// </summary>
    MiningBar,

    /// <summary>
    /// Work done when assembling Foobar.
    /// </summary>
    AssemblingFoobar,

    /// <summary>
    /// Work done when selling Foobar.
    /// </summary>
    SellingFoobar,

    /// <summary>
    /// Work done when buying a Robot.
    /// </summary>
    BuyingRobot,

    /// <summary>
    /// Work done to change activity.
    /// </summary>
    ChangingActivity,
}
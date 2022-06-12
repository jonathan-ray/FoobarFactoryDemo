namespace FoobarFactoryDemo.Models.Domain;

/// <summary>
/// Defines the state of a robot at any given time.
/// </summary>
/// <param name="Id">The ID of the robot.</param>
/// <param name="Activity">The activity performed by the robot.</param>
public record RobotState(int Id, ActivityType Activity);
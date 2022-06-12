namespace FoobarFactoryDemo.Models.Domain;

/// <summary>
/// Helper methods for activity weights.
/// </summary>
public interface IActivityWeights
{
    /// <summary>
    /// Gets the activity type based on their weighted values.
    /// </summary>
    /// <param name="selectorValue">A random value between 0 and 1 that corresponds to the bounds of an actionable activity's range.</param>
    /// <returns>The activity type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if selector value is not within the bounds of 0 to 1.</exception>
    ActivityType GetActionableActivity(double selectorValue);

    /// <summary>
    /// Determines whether to continue with the previous activity or change activity.
    /// </summary>
    /// <param name="selectorValue">A random value between 0 and 1 to decide the outcome.</param>
    /// <param name="previousActivity">The previous activity.</param>
    /// <returns>The next activity.</returns>
    ActivityType GetChangeableActivity(double selectorValue, ActivityType previousActivity);
}
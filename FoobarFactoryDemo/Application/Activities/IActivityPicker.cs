using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities;

/// <summary>
/// Logic for picking the next activity to be completed.
/// </summary>
public interface IActivityPicker
{
    /// <summary>
    /// Get the next activity to be completed, based on the previously completed activity.
    /// </summary>
    /// <param name="previousActivity">The previously completed activity.</param>
    /// <returns>The next activity.</returns>
    ActivityType GetNextActivity(ActivityType previousActivity);
}
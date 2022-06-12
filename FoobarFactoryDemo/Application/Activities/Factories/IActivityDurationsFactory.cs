using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Factories;

/// <summary>
/// Factory to create the durations of each activity.
/// </summary>
public interface IActivityDurationsFactory
{
    /// <summary>
    /// Create the durations of each activity.
    /// </summary>
    /// <returns>The activity durations.</returns>
    ActivityDurations CreateDurations();
}
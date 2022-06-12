using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Abstraction of an activity to be done.
/// </summary>
public interface IActivity
{
    /// <summary>
    /// The type of the activity.
    /// </summary>
    ActivityType Type { get; }

    /// <summary>
    /// Attempts to get the resources required to run the activity.
    /// </summary>
    /// <returns>The result of the retrieval.</returns>
    ResourceRetrievalResult GetRequiredResources();

    /// <summary>
    /// Runs the actions of the activity.
    /// </summary>
    /// <param name="requiredResources">The resources required to run.</param>
    /// <returns><c>True</c> if the run was successful, else <c>false</c>.</returns>
    bool Run(ResourceRetrievalResult requiredResources);
}
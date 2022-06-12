using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for changing to a different activity.
/// </summary>
public class ChangingActivity : IActivity
{
    public ActivityType Type => ActivityType.ChangingActivity;

    public ResourceRetrievalResult GetRequiredResources() => ResourceRetrievalResult.NoResourcesRequired;

    public bool Run(ResourceRetrievalResult requiredResources) => true;
}
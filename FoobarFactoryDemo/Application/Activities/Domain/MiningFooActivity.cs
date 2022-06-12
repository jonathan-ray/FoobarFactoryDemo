using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for mining Foo.
/// </summary>
/// <remarks>
/// Mine and store 1x Foo.
/// </remarks>
public class MiningFooActivity : IActivity
{
    private readonly IResourceRepository repository;

    public MiningFooActivity(IResourceRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ActivityType Type => ActivityType.MiningFoo;

    public ResourceRetrievalResult GetRequiredResources() => ResourceRetrievalResult.NoResourcesRequired;

    public bool Run(ResourceRetrievalResult resources)
    {
        this.repository.Store(ResourceType.Foo, 1);
        return true;
    }
}
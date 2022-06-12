using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for mining Bar.
/// </summary>
/// <remarks>
/// Mine and store 1x Bar.
/// </remarks>
public class MiningBarActivity : IActivity
{
    private readonly IResourceRepository repository;

    public MiningBarActivity(IResourceRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ActivityType Type => ActivityType.MiningBar;

    public ResourceRetrievalResult GetRequiredResources() => ResourceRetrievalResult.NoResourcesRequired;

    public bool Run(ResourceRetrievalResult resources)
    {
        this.repository.Store(ResourceType.Bar, 1);
        return true;
    }
}
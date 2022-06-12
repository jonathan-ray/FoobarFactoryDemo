using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for assembling a Foobar.
/// </summary>
/// <remarks>
/// Try to assemble 1x Foobar from 1x Foo and 1x Bar.
/// Activity has a 60% chance of success.
/// If activity fails, the Bar can be reused but the Foo cannot.
/// </remarks>
public class AssemblingFoobarActivity : IActivity
{
    private readonly IRandomGenerator randomGenerator;
    private readonly IResourceRepository repository;

    public AssemblingFoobarActivity(
        IRandomGenerator randomGenerator,
        IResourceRepository repository)
    {
        this.randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ActivityType Type => ActivityType.AssemblingFoobar;

    public ResourceRetrievalResult GetRequiredResources()
    {
        if (this.repository.TryGet(ResourceType.Foo, 1))
        {
            if (this.repository.TryGet(ResourceType.Bar, 1))
            {
                return new ResourceRetrievalResult(
                    true,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Foo, 1 },
                        { ResourceType.Bar, 1 }
                    });
            }

            this.repository.Store(ResourceType.Foo, 1);
        }

        return new ResourceRetrievalResult(false);
    }

    public bool Run(ResourceRetrievalResult resources)
    {
        if (this.randomGenerator.NextDouble() < 0.6)
        {
            this.repository.Store(ResourceType.Foobar, 1);
            return true;
        }
        else
        {
            this.repository.Store(ResourceType.Bar, 1);
            return false;
        }
    }
}
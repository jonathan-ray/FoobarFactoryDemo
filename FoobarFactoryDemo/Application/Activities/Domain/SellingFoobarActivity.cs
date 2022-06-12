using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for selling Foobar.
/// </summary>
/// <remarks>
/// Try to sell up to 5x Foobar at once. Each Foobar is worth 1 Euro.
/// </remarks>
public class SellingFoobarActivity : IActivity
{
    private readonly IResourceRepository repository;

    public SellingFoobarActivity(IResourceRepository repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public ActivityType Type => ActivityType.SellingFoobar;

    public ResourceRetrievalResult GetRequiredResources()
    {
        var foobarCount = this.repository.TryGetRange(ResourceType.Foobar, 1, 5);

        return new ResourceRetrievalResult(
            foobarCount > 0,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Foobar, foobarCount }
            });
    }

    public bool Run(ResourceRetrievalResult result)
    {
        if (!result.Resources.ContainsKey(ResourceType.Foobar) || result.Resources[ResourceType.Foobar] < 1)
        {
            return false;
        }

        this.repository.Store(ResourceType.Euro, GetPriceInEuros(result.Resources[ResourceType.Foobar]));

        return true;
    }

    private static int GetPriceInEuros(int foobarCount)
    {
        // Price in Euros is 1:1
        return foobarCount;
    }
}
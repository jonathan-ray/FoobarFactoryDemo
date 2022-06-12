using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Models;

/// <summary>
/// Encapsulation of the result of a retrieving required resources.
/// </summary>
public class ResourceRetrievalResult
{
    /// <summary>
    /// Result of no resources being required.
    /// </summary>
    public static ResourceRetrievalResult NoResourcesRequired = new(true);

    public ResourceRetrievalResult(bool wasSuccessful, IDictionary<ResourceType, int>? resources = null)
    {
        this.WasSuccessful = wasSuccessful;
        this.Resources = resources ?? new Dictionary<ResourceType, int>();
    }

    /// <summary>
    /// Was resource retrieval successful.
    /// </summary>
    public bool WasSuccessful { get; init; }

    /// <summary>
    /// The resources retrieved (if any).
    /// </summary>
    public IDictionary<ResourceType, int> Resources { get; set; }
}
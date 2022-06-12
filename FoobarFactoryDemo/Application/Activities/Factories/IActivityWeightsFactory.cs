using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Factories;

/// <summary>
/// Factory for creating weights of activities.
/// </summary>
public interface IActivityWeightsFactory
{
    /// <summary>
    /// Creates activity weights.
    /// </summary>
    /// <returns>The activity weights.</returns>
    IActivityWeights CreateActivityWeights();
}
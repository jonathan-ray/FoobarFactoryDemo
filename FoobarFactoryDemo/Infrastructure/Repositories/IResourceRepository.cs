using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Infrastructure.Repositories;

/// <summary>
/// Abstraction of a repository of resources.
/// </summary>
public interface IResourceRepository
{
    /// <summary>
    /// Stores a specified amount of a resource in the repository.
    /// </summary>
    /// <param name="type">The resource's type.</param>
    /// <param name="amount">The amount to store (1 or more).</param>
    /// <exception cref="ResourceRepositoryRequestAmountException">Thrown if the amount is less than 1.</exception>
    void Store(ResourceType type, int amount);

    /// <summary>
    /// Tries to retrieve a specified amount of a resource from the repository.
    /// </summary>
    /// <param name="type">The resource's type.</param>
    /// <param name="amount">The amount requested (1 or more).</param>
    /// <returns><c>True</c> if we successfully retrieved the amount, else <c>false</c>.</returns>
    /// <exception cref="ResourceRepositoryRequestAmountException">Thrown if the amount is less than 1.</exception>
    bool TryGet(ResourceType type, int amount);

    /// <summary>
    /// Tries to get as much of a resource as possible from the repository, given a specified range.
    /// </summary>
    /// <param name="type">The resource's type.</param>
    /// <param name="minimum">The minimum amount required.</param>
    /// <param name="maximum">The maximum amount required.</param>
    /// <returns>The amount successfully retrieved from the given range, else <c>0</c>.</returns>
    /// <exception cref="ResourceRepositoryRequestAmountException">Thrown if the minimum amount is less than 0.</exception>
    /// <exception cref="ResourceRepositoryRequestAmountRangeException">Thrown if the maximum amount is not greater than the minimum.</exception>
    int TryGetRange(ResourceType type, int minimum, int maximum);
}
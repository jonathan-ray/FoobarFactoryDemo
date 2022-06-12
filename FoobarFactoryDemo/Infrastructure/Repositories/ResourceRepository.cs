using System.Collections.Concurrent;
using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Infrastructure.Repositories;

public class ResourceRepository : IResourceRepository
{
    private readonly ConcurrentDictionary<ResourceType, int> repository;
    private readonly IPresentationService presentationService;

    public ResourceRepository(IPresentationService presentationService)
    {
        this.repository = new ConcurrentDictionary<ResourceType, int>();
        this.presentationService = presentationService ?? throw new ArgumentNullException(nameof(presentationService));
    }

    public void Store(ResourceType resource, int amount)
    {
        if (amount < 1)
        {
            throw new ResourceRepositoryRequestAmountException(amount);
        }

        var balance = this.repository.AddOrUpdate(resource, amount, (_, balance) => checked(balance + amount));
        this.presentationService.UpdateResourceBalance(resource, amount, balance);
    }

    public bool TryGet(ResourceType resource, int amount)
    {
        if (amount < 1)
        {
            throw new ResourceRepositoryRequestAmountException(amount);
        }

        var success = false;
        var balance = this.repository.AddOrUpdate(resource, 0, (_, balance) =>
        {
            if (balance >= amount)
            {
                success = true;
                return balance - amount;
            }

            return 0;
        });

        // Only update if we successfully retrieved the total request amount.
        if (success)
        {
            this.presentationService.UpdateResourceBalance(resource, -amount, balance);
        }

        return success;
    }

    public int TryGetRange(ResourceType resource, int minimum, int maximum)
    {
        if (minimum < 0)
        {
            throw new ResourceRepositoryRequestAmountException(minimum);
        }

        if (minimum >= maximum)
        {
            throw new ResourceRepositoryRequestAmountRangeException(minimum, maximum);
        }

        var amountRetrieved = 0;
        var balance = this.repository.AddOrUpdate(resource, 0, (_, balance) =>
        {
            if (balance >= minimum)
            {
                if (balance >= maximum)
                {
                    amountRetrieved = maximum;
                    return balance - maximum;
                }
                amountRetrieved = balance;
                return 0;
            }

            return balance;
        });

        // Only provide an update if we successfully retrieved at least the minimum amount.
        if (minimum > 0 && amountRetrieved >= minimum)
        {
            this.presentationService.UpdateResourceBalance(resource, -amountRetrieved, balance);
        }

        return amountRetrieved;
    }
}
using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Application.Activities.Domain;

/// <summary>
/// Activity for buying a robot.
/// </summary>
/// <remarks>
/// Try to buy a robot for the price of 3 Euros and 6 Foo.
/// </remarks>
public class BuyingRobotActivity : IActivity
{
    private readonly IResourceRepository repository;
    private readonly IProductionLine<RobotState> productionLine;

    public ActivityType Type => ActivityType.BuyingRobot;

    public BuyingRobotActivity(
        IResourceRepository repository,
        IProductionLine<RobotState> productionLine)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.productionLine = productionLine ?? throw new ArgumentNullException(nameof(productionLine));
    }

    public ResourceRetrievalResult GetRequiredResources()
    {
        if (this.repository.TryGet(ResourceType.Euro, 3))
        {
            if (this.repository.TryGet(ResourceType.Foo, 6))
            {
                return new ResourceRetrievalResult(
                    true,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Euro, 3 },
                        { ResourceType.Foo, 6 }
                    });
            }

            this.repository.Store(ResourceType.Euro, 3);
        }

        return new ResourceRetrievalResult(false);
    }

    public bool Run(ResourceRetrievalResult resources)
    {
        this.productionLine.AddWorker();
        return true;
    }
}
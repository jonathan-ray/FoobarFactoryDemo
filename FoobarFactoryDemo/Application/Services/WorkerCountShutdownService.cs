using FoobarFactoryDemo.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Application.Services;

/// <summary>
/// Defines automatic shutdown logic based on reaching a maximum worker count.
/// </summary>
public class WorkerCountShutdownService<TWorkerState> : IAutoShutdownService
{
    private readonly IProductionLine<TWorkerState> productionLine;
    private readonly FactorySettings factorySettings;

    public WorkerCountShutdownService(
        IProductionLine<TWorkerState> productionLine,
        IOptions<FactorySettings> factorySettings)
    {
        this.productionLine = productionLine ?? throw new ArgumentNullException(nameof(productionLine));
        this.factorySettings = factorySettings?.Value ?? throw new ArgumentNullException(nameof(factorySettings));
    }

    public bool ShouldShutdown => this.productionLine.WorkerCount >= this.factorySettings.ShutdownOnWorkerCount;
}
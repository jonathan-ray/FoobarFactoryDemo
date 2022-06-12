using FoobarFactoryDemo;
using FoobarFactoryDemo.Application.Activities;
using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Application.Activities.Services;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;
using FoobarFactoryDemo.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Set up configuration:
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
    .AddJsonFile("appsettings.json", false)
    .Build();

// Set up instance container registration:
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            // Configuration settings found in appsettings.json:
            .Configure<ActivityDurationSettings>(configuration.GetSection(nameof(ActivityDurationSettings)))
            .Configure<FactorySettings>(configuration.GetSection(nameof(FactorySettings)))
            .Configure<WeightedActivityPickerSettings>(configuration.GetSection(nameof(WeightedActivityPickerSettings)))

            // Activity services
            .AddSingleton<IActivityDurationsFactory, ActivityDurationsFactory>()
            .AddSingleton<IActivityWeightsFactory, ActivityWeightsFactory>()
            .AddSingleton<IActivityPicker, WeightedActivityPicker>()

            // Robot services
            .AddSingleton<IProductionLine<RobotState>, RobotProductionLine>()
            .AddSingleton<IWorkOrchestratorFactory<RobotState>, RobotWorkOrchestratorFactory>()

            // Generic services
            .AddSingleton<ISchedulerService, SchedulerService<RobotState>>()
            .AddSingleton<IAutoShutdownService, WorkerCountShutdownService<RobotState>>()

            // Infrastructure
            .AddSingleton<IResourceRepository, ResourceRepository>()

            // Presentation
            .AddSingleton<IWorkerPresentationService<RobotState>, ConsolePresentationService>()
            .AddSingleton<IPresentationService, ConsolePresentationService>()

            // Utilities
            .AddSingleton<IRandomGenerator, RandomGenerator>()

            // Containing instance.
            .AddSingleton<IFoobarFactory, FoobarFactory>())
    .Build();

// Run:
var foobarFactory = host.Services.GetRequiredService<IFoobarFactory>();
await foobarFactory.Run();

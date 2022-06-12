using FoobarFactoryDemo.Application.Activities;
using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Factories;

[Trait("Category", "UnitTests")]
public class RobotWorkOrchestratorFactoryTests
{
    [Fact]
    public void Construction_WithNullActivityPicker_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestratorFactory(
            activityPicker: null!,
            Mock.Of<IActivityDurationsFactory>(),
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityPicker");
    }

    [Fact]
    public void Construction_WithNullDurationsFactory_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestratorFactory(
            Mock.Of<IActivityPicker>(),
            activityDurationsFactory: null!,
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityDurationsFactory");
    }

    [Fact]
    public void Construction_WithNullRandomGenerator_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestratorFactory(
            Mock.Of<IActivityPicker>(),
            Mock.Of<IActivityDurationsFactory>(),
            randomGenerator: null!,
            Mock.Of<IResourceRepository>(),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("randomGenerator");
    }

    [Fact]
    public void Construction_WithNullResourceRepository_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestratorFactory(
            Mock.Of<IActivityPicker>(),
            Mock.Of<IActivityDurationsFactory>(),
            Mock.Of<IRandomGenerator>(),
            resourceRepository: null!,
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("resourceRepository");
    }

    [Fact]
    public void Construction_WithNullPresentationService_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestratorFactory(
            Mock.Of<IActivityPicker>(),
            Mock.Of<IActivityDurationsFactory>(),
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            presentationService: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("presentationService");
    }

    [Fact]
    public void CreateWork_WithValidDependencies_ShouldCreateSuccessfully()
    {
        var durationsFactory = new Mock<IActivityDurationsFactory>();
        durationsFactory
            .Setup(m => m.CreateDurations())
            .Returns(new ActivityDurations(
                TimeSpan.Zero,
                new VariableTimeSpan(Mock.Of<IRandomGenerator>(), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)),
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero));

        var factoryUnderTest = new RobotWorkOrchestratorFactory(
            Mock.Of<IActivityPicker>(),
            durationsFactory.Object,
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        var orchestrator = factoryUnderTest.CreateWork(Mock.Of<IProductionLine<RobotState>>());

        orchestrator.Should().NotBeNull();
    }
}
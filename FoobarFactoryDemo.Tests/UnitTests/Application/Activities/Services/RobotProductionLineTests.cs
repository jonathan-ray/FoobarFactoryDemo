using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Application.Activities.Services;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Services;

[Trait("Category", "UnitTests")]
public class RobotProductionLineTests
{
    private static readonly FactorySettings DefaultSettings = new()
    {
        InitialWorkerCount = 3
    };

    private readonly Mock<IWorkOrchestratorFactory<RobotState>> workOrchestratorFactoryMock;
    private readonly Mock<IWorkOrchestrator<RobotState>> workOrchestratorMock;

    private readonly IProductionLine<RobotState> productionLineUnderTest;

    public RobotProductionLineTests()
    {
        this.workOrchestratorFactoryMock = new Mock<IWorkOrchestratorFactory<RobotState>>();
        this.workOrchestratorMock = new Mock<IWorkOrchestrator<RobotState>>();
        this.workOrchestratorFactoryMock
            .Setup(m => m.CreateWork(It.IsAny<IProductionLine<RobotState>>()))
            .Returns(this.workOrchestratorMock.Object);

        this.productionLineUnderTest = new RobotProductionLine(this.workOrchestratorFactoryMock.Object,
            Options.Create(DefaultSettings),
            Mock.Of<IWorkerPresentationService<RobotState>>());
    }

    [Fact]
    public void Construction_WithNullWorkOrchestratorFactory_ShouldThrow()
    {
        var construction = () => new RobotProductionLine(
            workOrchestratorFactory: null!,
            Options.Create(DefaultSettings),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("workOrchestratorFactory");
    }

    [Fact]
    public void Construction_WithNullFactorySettingsOptions_ShouldThrow()
    {
        var construction = () => new RobotProductionLine(
            Mock.Of<IWorkOrchestratorFactory<RobotState>>(),
            factorySettings: null!,
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("factorySettings");
    }

    [Fact]
    public void Construction_WithNullFactorySettings_ShouldThrow()
    {
        var construction = () => new RobotProductionLine(
            Mock.Of<IWorkOrchestratorFactory<RobotState>>(),
            factorySettings: Options.Create<FactorySettings>(null!),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("factorySettings");
    }

    [Fact]
    public void Construction_WithNullPresentationService_ShouldThrow()
    {
        var construction = () => new RobotProductionLine(
            Mock.Of<IWorkOrchestratorFactory<RobotState>>(),
            Options.Create(DefaultSettings),
            presentationService: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("presentationService");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Construction_WithInvalidInitialWorkerCount_ShouldThrow(int workerCount)
    {
        var construction = () => new RobotProductionLine(
            Mock.Of<IWorkOrchestratorFactory<RobotState>>(),
            factorySettings: Options.Create(new FactorySettings { InitialWorkerCount = workerCount }),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("factorySettings");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    public void WorkerCount_WithValidSettings_ShouldReturnSameValue(int workerCount)
    {
        var productionLine = new RobotProductionLine(
            Mock.Of<IWorkOrchestratorFactory<RobotState>>(),
            factorySettings: Options.Create(new FactorySettings { InitialWorkerCount = workerCount }),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        productionLine.WorkerCount.Should().Be(workerCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    public void Initialize_WithValidInitialWorkerCount_ShouldCreateSameAmountOfActivities(int workerCount)
    {
        var productionLine = new RobotProductionLine(
            this.workOrchestratorFactoryMock.Object,
            factorySettings: Options.Create(new FactorySettings { InitialWorkerCount = workerCount }),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        productionLine.Initialize();

        this.workOrchestratorMock.Verify(m => 
            m.CreateInitialState(It.IsAny<int>()), 
            Times.Exactly(workerCount));
    }

    [Fact]
    public void CreateNewActivityFromPrevious_WithValidState_ShouldAddIncompleteActivity()
    {
        this.productionLineUnderTest.HasIncompleteActivities.Should().BeFalse();

        var state = new RobotState(0, ActivityType.AssemblingFoobar);

        this.workOrchestratorMock
            .Setup(m => m.Run(state))
            .ReturnsAsync(new RobotState(0, ActivityType.ChangingActivity));

        this.productionLineUnderTest.CreateNewActivityFromPrevious(state);

        this.productionLineUnderTest.HasIncompleteActivities.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    public void AddWorker_WithoutIncompleteActivities_ShouldJustIncrementWorkerCount(int workersToAdd)
    {
        for (var i = 0; i < workersToAdd; i++)
        {
            this.productionLineUnderTest.AddWorker();
        }

        this.productionLineUnderTest.WorkerCount.Should().Be(DefaultSettings.InitialWorkerCount + workersToAdd);
    }

    [Fact]
    public void AddWorker_WithIncompleteActivities_ShouldStartActivity()
    {
        var state = new RobotState(0, ActivityType.AssemblingFoobar);
        this.workOrchestratorMock
            .Setup(m => m.Run(state))
            .ReturnsAsync(new RobotState(0, ActivityType.ChangingActivity));
        this.productionLineUnderTest.CreateNewActivityFromPrevious(state);

        var newWorkerState = new RobotState(DefaultSettings.InitialWorkerCount, ActivityType.Idling);
        this.workOrchestratorMock
            .Setup(m => m.CreateInitialState(DefaultSettings.InitialWorkerCount))
            .Returns(newWorkerState);

        this.productionLineUnderTest.AddWorker();

        this.workOrchestratorMock
            .Verify(m => m.CreateInitialState(DefaultSettings.InitialWorkerCount),
            Times.Once);

        this.workOrchestratorMock
            .Verify(m => m.Run(newWorkerState),
            Times.Once);
    }

    [Fact]
    public async Task NextCompletedActivity_WithIncompleteActivities_ShouldPopCompleteActivity()
    {
        var state = new RobotState(0, ActivityType.AssemblingFoobar);
        var newState = new RobotState(0, ActivityType.ChangingActivity);
        this.workOrchestratorMock
            .Setup(m => m.Run(state))
            .ReturnsAsync(newState);
        this.productionLineUnderTest.CreateNewActivityFromPrevious(state);

        this.productionLineUnderTest.HasIncompleteActivities.Should().BeTrue();

        var completedActivity = await this.productionLineUnderTest.NextCompletedActivity();

        completedActivity.Should().Be(newState);

        this.productionLineUnderTest.HasIncompleteActivities.Should().BeFalse();
    }

    [Fact]
    public async Task NextCompletedActivity_WithoutIncompleteActivities_ShouldPopCompleteActivity()
    {
        await this.productionLineUnderTest
            .Awaiting(l => l.NextCompletedActivity())
            .Should().ThrowAsync<EmptyProductionLineException>();
    }
}
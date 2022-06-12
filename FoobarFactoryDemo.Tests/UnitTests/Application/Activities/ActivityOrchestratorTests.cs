using FoobarFactoryDemo.Application.Activities;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities;

[Trait("Category", "UnitTests")]
public class ActivityOrchestratorTests
{
    private readonly Mock<IRandomGenerator> randomGeneratorMock;
    private readonly Mock<IResourceRepository> repositoryMock;

    private readonly IActivityOrchestrator orchestratorUnderTest;

    public ActivityOrchestratorTests()
    {
        this.randomGeneratorMock = new Mock<IRandomGenerator>();
        this.repositoryMock = new Mock<IResourceRepository>();

        this.orchestratorUnderTest = new ActivityOrchestrator(
            new ActivityDurations(
                TimeSpan.Zero,
                new VariableTimeSpan(this.randomGeneratorMock.Object, TimeSpan.Zero, TimeSpan.FromTicks(1)),
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero),
            this.randomGeneratorMock.Object,
            this.repositoryMock.Object,
            Mock.Of<IProductionLine<RobotState>>());
    }

    [Fact]
    public void Construction_WithNullDurations_ShouldThrow()
    {
        var construction = () => new ActivityOrchestrator(
            activityDurations: null!,
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            Mock.Of<IProductionLine<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityDurations");
    }

    [Fact]
    public void Construction_WithNullRandomGenerator_ShouldThrow()
    {
        var construction = () => new ActivityOrchestrator(
            new ActivityDurations(
                TimeSpan.Zero,
                new VariableTimeSpan(this.randomGeneratorMock.Object, TimeSpan.Zero, TimeSpan.FromTicks(1)),
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero),
            randomGenerator: null!,
            Mock.Of<IResourceRepository>(),
            Mock.Of<IProductionLine<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("randomGenerator");
    }

    [Fact]
    public void Construction_WithNullResourceRepository_ShouldThrow()
    {
        var construction = () => new ActivityOrchestrator(
            new ActivityDurations(
                TimeSpan.Zero,
                new VariableTimeSpan(this.randomGeneratorMock.Object, TimeSpan.Zero, TimeSpan.FromTicks(1)),
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero),
            Mock.Of<IRandomGenerator>(),
            resourceRepository: null!,
            Mock.Of<IProductionLine<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("resourceRepository");
    }

    [Fact]
    public void Construction_WithNullProductionLine_ShouldThrow()
    {
        var construction = () => new ActivityOrchestrator(
            new ActivityDurations(
                TimeSpan.Zero,
                new VariableTimeSpan(this.randomGeneratorMock.Object, TimeSpan.Zero, TimeSpan.FromTicks(1)),
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero,
                TimeSpan.Zero),
            Mock.Of<IRandomGenerator>(),
            Mock.Of<IResourceRepository>(),
            productionLine: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("productionLine");
    }

    [Theory]
    [InlineData(ActivityType.ChangingActivity, true)]
    [InlineData(ActivityType.AssemblingFoobar, false)]
    [InlineData(ActivityType.BuyingRobot, false)]
    [InlineData(ActivityType.MiningBar, true)]
    [InlineData(ActivityType.MiningFoo, true)]
    [InlineData(ActivityType.SellingFoobar, true)]
    public async Task Run_WithSupportedActivities_ShouldReturnExpectedBehaviour(ActivityType activity, bool isSuccessful)
    {
        this.randomGeneratorMock
            .Setup(m => m.NextDouble())
            .Returns(0.5);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Foo, It.IsAny<int>()))
            .Returns(true);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Bar, It.IsAny<int>()))
            .Returns(false);

        this.repositoryMock
            .Setup(m => m.TryGetRange(ResourceType.Foobar, It.IsAny<int>(), It.IsAny<int>()))
            .Returns(3);

        var state = new RobotState(321, activity);

        var result = await this.orchestratorUnderTest.Run(state);

        result.Should().Be(isSuccessful);
    }

    [Theory]
    [InlineData(ActivityType.Idling)]
    [InlineData((ActivityType)4123)]
    public async Task Run_WithUnsupportedActivities_ShouldThrow(ActivityType activity)
    {
        var state = new RobotState(321, activity);

        await this.orchestratorUnderTest
            .Awaiting(o => o.Run(state))
            .Should().ThrowAsync<NotSupportedException>();
    }
}
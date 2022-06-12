using FoobarFactoryDemo.Application.Activities;
using FoobarFactoryDemo.Application.Activities.Services;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Services;

[Trait("Category", "UnitTests")]
public class RobotWorkOrchestratorTests
{
    private readonly Mock<IActivityPicker> pickerMock;
    private readonly Mock<IActivityOrchestrator> activityOrchestratorMock;
    private readonly Mock<IWorkerPresentationService<RobotState>> presentationMock;

    private readonly IWorkOrchestrator<RobotState> orchestratorUnderTest;

    public RobotWorkOrchestratorTests()
    {
        this.pickerMock = new Mock<IActivityPicker>();
        this.activityOrchestratorMock = new Mock<IActivityOrchestrator>();
        this.presentationMock = new Mock<IWorkerPresentationService<RobotState>>();

        this.orchestratorUnderTest = new RobotWorkOrchestrator(
            this.pickerMock.Object,
            this.activityOrchestratorMock.Object,
            this.presentationMock.Object);
    }

    [Fact]
    public void Construction_WithNullActivityPicker_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestrator(
            activityPicker: null!,
            Mock.Of<IActivityOrchestrator>(),
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityPicker");
    }

    [Fact]
    public void Construction_WithNullActivityOrchestrator_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestrator(
            Mock.Of<IActivityPicker>(),
            activityOrchestrator: null!,
            Mock.Of<IWorkerPresentationService<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityOrchestrator");
    }

    [Fact]
    public void Construction_WithNullWorkerPresentationService_ShouldThrow()
    {
        var construction = () => new RobotWorkOrchestrator(
            Mock.Of<IActivityPicker>(),
            Mock.Of<IActivityOrchestrator>(),
            presentationService: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("presentationService");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(8)]
    public void CreateInitialState_ShouldReturnDefault(int id)
    {
        var state = this.orchestratorUnderTest.CreateInitialState(id);

        state.Id.Should().Be(id);
        state.Activity.Should().Be(ActivityType.Idling);
    }

    [Fact]
    public async Task Run_WithSuccess_ShouldDeclareCompleted()
    {
        await this.SetupAndExecuteRun(true);

        this.presentationMock.Verify(p =>
            p.ActivityCompleted(It.IsAny<RobotState>()),
            Times.Once);
    }

    [Fact]
    public async Task Run_WithFailure_ShouldDeclareFailure()
    {
        await this.SetupAndExecuteRun(false);

        this.presentationMock.Verify(p =>
            p.ActivityFailed(It.IsAny<RobotState>()),
            Times.Once);
    }

    private async Task SetupAndExecuteRun(bool activityOrchestratorResult)
    {
        const ActivityType activityType = ActivityType.AssemblingFoobar;

        this.pickerMock
            .Setup(m => m.GetNextActivity(It.IsAny<ActivityType>()))
            .Returns(activityType);

        this.activityOrchestratorMock
            .Setup(m => m.Run(It.IsAny<RobotState>()))
            .ReturnsAsync(activityOrchestratorResult);

        const int id = 123;
        var result = await this.orchestratorUnderTest.Run(new RobotState(id, ActivityType.Idling));

        result.Id.Should().Be(id);
        result.Activity.Should().Be(activityType);
    }
}
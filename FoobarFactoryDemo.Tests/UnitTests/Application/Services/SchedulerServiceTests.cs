using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Services;

[Trait("Category", "UnitTests")]
public class SchedulerServiceTests
{
    private readonly Mock<IAutoShutdownService> shutdownServiceMock;
    private readonly Mock<IProductionLine<RobotState>> productionLineMock;
    private readonly Mock<IPresentationService> presentationServiceMock;

    private readonly ISchedulerService serviceUnderTest;
    public SchedulerServiceTests()
    {
        this.shutdownServiceMock = new Mock<IAutoShutdownService>();
        this.productionLineMock = new Mock<IProductionLine<RobotState>>();
        this.presentationServiceMock = new Mock<IPresentationService>();

        this.serviceUnderTest = new SchedulerService<RobotState>(
            this.shutdownServiceMock.Object,
            this.productionLineMock.Object,
            this.presentationServiceMock.Object);
    }

    [Fact]
    public void Construction_WithNullShutdownService_ShouldThrow()
    {
        var construction = () => new SchedulerService<RobotState>(
            shutdownService: null!,
            Mock.Of<IProductionLine<RobotState>>(),
            Mock.Of<IPresentationService>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("shutdownService");
    }

    [Fact]
    public void Construction_WithNullProductionLine_ShouldThrow()
    {
        var construction = () => new SchedulerService<RobotState>(
            Mock.Of<IAutoShutdownService>(),
            productionLine: null!,
            Mock.Of<IPresentationService>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("productionLine");
    }

    [Fact]
    public void Construction_WithNullPresentationService_ShouldThrow()
    {
        var construction = () => new SchedulerService<RobotState>(
            Mock.Of<IAutoShutdownService>(),
            Mock.Of<IProductionLine<RobotState>>(),
            presentationService: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("presentationService");
    }

    [Fact]
    public async Task Start_WhileHasIncompleteActivities_ShouldIterate()
    {
        this.shutdownServiceMock
            .Setup(s => s.ShouldShutdown)
            .Returns(false);

        this.productionLineMock
            .SetupSequence(l => l.HasIncompleteActivities)
            .Returns(true)
            .Returns(true)
            .Returns(false);

        var completedState = new RobotState(1, ActivityType.BuyingRobot);

        this.productionLineMock
            .Setup(m => m.NextCompletedActivity())
            .ReturnsAsync(completedState);

        await this.serviceUnderTest.Start();

        this.productionLineMock.Verify(l => l.Initialize(), Times.Once);
        this.productionLineMock.Verify(l => l.NextCompletedActivity(), Times.Exactly(2));
        this.productionLineMock.Verify(l => l.CreateNewActivityFromPrevious(completedState), Times.Exactly(2));
    }

    [Fact]
    public async Task Start_WhenShouldShutdown_ShouldNotCreateNew()
    {
        this.shutdownServiceMock
            .Setup(s => s.ShouldShutdown)
            .Returns(true);

        this.productionLineMock
            .SetupSequence(l => l.HasIncompleteActivities)
            .Returns(true)
            .Returns(true)
            .Returns(false);

        var completedState = new RobotState(1, ActivityType.BuyingRobot);

        this.productionLineMock
            .Setup(m => m.NextCompletedActivity())
            .ReturnsAsync(completedState);

        await this.serviceUnderTest.Start();

        this.presentationServiceMock.Verify(p => p.ShutdownInitiated(), Times.Once);
        this.productionLineMock.Verify(l => l.CreateNewActivityFromPrevious(It.IsAny<RobotState>()), Times.Never);
    }
}
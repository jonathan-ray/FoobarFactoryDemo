using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Application.Services;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Domain;

[Trait("Category", "UnitTests")]
public class BuyingRobotActivityTests
{
    private static readonly ResourceRetrievalResult SuccessfulResult = new(
        true,
        new Dictionary<ResourceType, int>
        {
            { ResourceType.Euro, 3 },
            { ResourceType.Foo, 6 }
        });

    private readonly Mock<IResourceRepository> repositoryMock;
    private readonly Mock<IProductionLine<RobotState>> productionLineMock;

    private readonly IActivity activityUnderTest;

    public BuyingRobotActivityTests()
    {
        this.repositoryMock = new Mock<IResourceRepository>();
        this.productionLineMock = new Mock<IProductionLine<RobotState>>();

        this.activityUnderTest = new BuyingRobotActivity(
            this.repositoryMock.Object,
            this.productionLineMock.Object);
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new BuyingRobotActivity(
            repository: null!,
            Mock.Of<IProductionLine<RobotState>>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Fact]
    public void Construction_WithNullProductionLine_ShouldThrow()
    {
        var construction = () => new BuyingRobotActivity(
            Mock.Of<IResourceRepository>(),
            productionLine: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("productionLine");
    }

    [Fact]
    public void GetRequiredResources_NeitherFooNorEurosAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(It.IsAny<ResourceType>(), It.IsAny<int>()))
            .Returns(false);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_JustFooAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Foo, 6))
            .Returns(true);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Euro, It.IsAny<int>()))
            .Returns(false);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_JustEurosAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Foo, It.IsAny<int>()))
            .Returns(false);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Euro, 3))
            .Returns(true);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_BothFooAndEurosAvailable_ShouldReturnSuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(It.IsAny<ResourceType>(), It.IsAny<int>()))
            .Returns(true);

        var result = this.activityUnderTest.GetRequiredResources();

        result.Should().BeEquivalentTo(SuccessfulResult);
    }

    [Fact]
    public void Run_ShouldAddWorker()
    {
        var result = this.activityUnderTest.Run(SuccessfulResult);

        result.Should().BeTrue();

        this.productionLineMock.Verify(l =>
            l.AddWorker(),
            Times.Once);
    }
}
using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Domain;

[Trait("Category", "UnitTests")]
public class AssemblingFoobarActivityTests
{
    private static readonly ResourceRetrievalResult SuccessfulResult = new(
        true,
        new Dictionary<ResourceType, int>
        {
            { ResourceType.Foo, 1 },
            { ResourceType.Bar, 1 }
        });

    private readonly Mock<IRandomGenerator> randomGeneratorMock;
    private readonly Mock<IResourceRepository> repositoryMock;

    private readonly IActivity activityUnderTest;

    public AssemblingFoobarActivityTests()
    {
        this.randomGeneratorMock = new Mock<IRandomGenerator>();
        this.repositoryMock = new Mock<IResourceRepository>();

        this.activityUnderTest = new AssemblingFoobarActivity(
            this.randomGeneratorMock.Object,
            this.repositoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullRandomGenerator_ShouldThrow()
    {
        var construction = () => new AssemblingFoobarActivity(
            randomGenerator: null!,
            Mock.Of<IResourceRepository>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("randomGenerator");
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new AssemblingFoobarActivity(
            Mock.Of<IRandomGenerator>(),
            repository: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Fact]
    public void GetRequiredResources_NeitherFooNorBarAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(It.IsAny<ResourceType>(), 1))
            .Returns(false);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_JustFooAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Foo, 1))
            .Returns(true);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Bar, 1))
            .Returns(false);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_JustBarAvailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Foo, 1))
            .Returns(false);

        this.repositoryMock
            .Setup(m => m.TryGet(ResourceType.Bar, 1))
            .Returns(true);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Fact]
    public void GetRequiredResources_BothFooAndBarAvailable_ShouldReturnSuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGet(It.IsAny<ResourceType>(), 1))
            .Returns(true);

        var result = this.activityUnderTest.GetRequiredResources();

        result.Should().BeEquivalentTo(SuccessfulResult);
    }

    [Fact]
    public void Run_WithRandomValueAsSuccess_ShouldReturnSuccess()
    {
        var iterations = 0;
        for (var i = 0.0; i < 0.6; i += 0.01)
        {
            this.randomGeneratorMock
                .Setup(m => m.NextDouble())
                .Returns(i);

            var result = this.activityUnderTest.Run(SuccessfulResult);

            result.Should().Be(true);

            iterations++;
        }

        this.repositoryMock.Verify(r =>
            r.Store(ResourceType.Foobar, 1),
            Times.Exactly(iterations));
    }

    [Fact]
    public void Run_WithRandomValueAsFailure_ShouldReturnFailure()
    {
        var iterations = 0;
        for (var i = 0.6; i < 1; i += 0.01)
        {
            this.randomGeneratorMock
                .Setup(m => m.NextDouble())
                .Returns(i);

            var result = this.activityUnderTest.Run(SuccessfulResult);

            result.Should().Be(false);

            iterations++;
        }

        this.repositoryMock.Verify(r =>
                r.Store(ResourceType.Bar, 1),
            Times.Exactly(iterations));
    }
}
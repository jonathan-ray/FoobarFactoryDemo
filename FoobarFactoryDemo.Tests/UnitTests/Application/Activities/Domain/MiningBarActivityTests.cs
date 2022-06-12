using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Domain;

[Trait("Category", "UnitTests")]
public class MiningBarActivityTests
{
    private readonly Mock<IResourceRepository> repositoryMock;

    private readonly IActivity activityUnderTest;

    public MiningBarActivityTests()
    {
        this.repositoryMock = new Mock<IResourceRepository>();

        this.activityUnderTest = new MiningBarActivity(this.repositoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new MiningBarActivity(
            repository: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Fact]
    public void GetRequiredResources_ShouldRequireNoResources()
    {
        var actualResources = this.activityUnderTest.GetRequiredResources();

        actualResources.Should().NotBeNull();
        actualResources.Should().BeEquivalentTo(ResourceRetrievalResult.NoResourcesRequired);
    }

    [Fact]
    public void Run_ShouldReturnSuccess()
    {
        var result = this.activityUnderTest.Run(ResourceRetrievalResult.NoResourcesRequired);

        result.Should().BeTrue();

        this.repositoryMock.Verify(m =>
            m.Store(ResourceType.Bar, 1),
            Times.Once);
    }
}
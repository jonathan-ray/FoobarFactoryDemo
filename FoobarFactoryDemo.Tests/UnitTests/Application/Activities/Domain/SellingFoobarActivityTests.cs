using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Activities.Models;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Domain;

[Trait("Category", "UnitTests")]
public class SellingFoobarActivityTests
{
    private readonly Mock<IResourceRepository> repositoryMock;

    private readonly IActivity activityUnderTest;

    public SellingFoobarActivityTests()
    {
        this.repositoryMock = new Mock<IResourceRepository>();

        this.activityUnderTest = new SellingFoobarActivity(this.repositoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullRepository_ShouldThrow()
    {
        var construction = () => new SellingFoobarActivity(
            repository: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("repository");
    }

    [Fact]
    public void GetRequiredResources_WithFoobarsUnavailable_ShouldReturnUnsuccessfully()
    {
        this.repositoryMock
            .Setup(m => m.TryGetRange(ResourceType.Foobar, It.IsAny<int>(), It.IsAny<int>()))
            .Returns(0);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void GetRequiredResources_WithFoobarsAvailable_ShouldReturnAmountAvailableSuccessfully(int amountAvailable)
    {
        this.repositoryMock
            .Setup(m => m.TryGetRange(ResourceType.Foobar, 1, 5))
            .Returns(amountAvailable);

        var result = this.activityUnderTest.GetRequiredResources();

        result.WasSuccessful.Should().BeTrue();
        result.Resources[ResourceType.Foobar].Should().Be(amountAvailable);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Run_WithFoobarsRetrieved_ShouldSellAll(int amountAvailable)
    {
        var result = this.activityUnderTest.Run(new ResourceRetrievalResult(
            true,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Foobar, amountAvailable }
            }));

        result.Should().BeTrue();

        this.repositoryMock.Verify(m =>
            m.Store(ResourceType.Euro, amountAvailable),
            Times.Once);
    }

    [Fact]
    public void Run_WithNoFoobarsRetrieved_ShouldReturnUnsuccessfully()
    {
        var result = this.activityUnderTest.Run(new ResourceRetrievalResult(
            true,
            new Dictionary<ResourceType, int>
            {
                { ResourceType.Foobar, 0 }
            }));

        result.Should().BeFalse();

        this.repositoryMock.Verify(m =>
            m.Store(ResourceType.Euro, It.IsAny<int>()),
            Times.Never);
    }

    [Fact]
    public void Run_WithNoEmptyResources_ShouldReturnUnsuccessfully()
    {
        var result = this.activityUnderTest.Run(ResourceRetrievalResult.NoResourcesRequired);

        result.Should().BeFalse();

        this.repositoryMock.Verify(m =>
                m.Store(ResourceType.Euro, It.IsAny<int>()),
            Times.Never);
    }
}
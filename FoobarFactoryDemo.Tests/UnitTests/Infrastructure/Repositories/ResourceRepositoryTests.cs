using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Infrastructure.Repositories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Presentation;

namespace FoobarFactoryDemo.Tests.UnitTests.Infrastructure.Repositories;

[Trait("Category", "UnitTests")]
public class ResourceRepositoryTests
{
    private readonly Mock<IPresentationService> presentationServiceMock;

    private readonly IResourceRepository repositoryUnderTest;

    public ResourceRepositoryTests()
    {
        this.presentationServiceMock = new Mock<IPresentationService>();

        this.repositoryUnderTest = new ResourceRepository(this.presentationServiceMock.Object);
    }

    [Fact]
    public void Construction_WithNullPresentationService_ShouldThrow()
    {
        var construction = () => new ResourceRepository(presentationService: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("presentationService");
    }

    [Fact]
    public void Store_WithEmptyRepository_ShouldStoreSuccessfully()
    {
        this.repositoryUnderTest.Store(ResourceType.Bar, 1);

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Bar, 1, 1),
            Times.Once);
    }

    [Fact]
    public void Store_WithEmptyRepositoryForResource_ShouldStoreSuccessfully()
    {
        this.repositoryUnderTest.Store(ResourceType.Euro, 12);

        this.repositoryUnderTest.Store(ResourceType.Bar, 1);

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Bar, 1, 1),
            Times.Once);
    }

    [Fact]
    public void Store_WithPopulatedRepository_ShouldStoreSuccessfully()
    {
        this.repositoryUnderTest.Store(ResourceType.Bar, 12);

        this.repositoryUnderTest.Store(ResourceType.Bar, 1);

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Bar, 1, 13),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Store_WithInvalidAmount_ShouldThrow(int invalidAmount)
    {
        this.repositoryUnderTest
            .Invoking(r => r.Store(ResourceType.Bar, invalidAmount))
            .Should().Throw<ResourceRepositoryRequestAmountException>();
    }

    [Fact]
    public void Store_WithIntegerOverflowAmount_ShouldThrow()
    {
        this.repositoryUnderTest.Store(ResourceType.Bar, int.MaxValue);

        this.repositoryUnderTest
            .Invoking(r => r.Store(ResourceType.Bar, 1))
            .Should().Throw<OverflowException>();
    }

    [Fact]
    public void TryGet_WithEmptyRepository_ShouldReturnUnsuccessful()
    {
        var actualResult = this.repositoryUnderTest.TryGet(ResourceType.Euro, 1);

        actualResult.Should().BeFalse();
    }

    [Fact]
    public void TryGet_WithEmptyRepositoryForResource_ShouldReturnUnsuccessful()
    {
        this.repositoryUnderTest.Store(ResourceType.Bar, int.MaxValue);

        var actualResult = this.repositoryUnderTest.TryGet(ResourceType.Euro, 1);

        actualResult.Should().BeFalse();
    }

    [Fact]
    public void TryGet_WithNotEnoughInRepository_ShouldReturnUnsuccessful()
    {
        this.repositoryUnderTest.Store(ResourceType.Euro, 1);

        var actualResult = this.repositoryUnderTest.TryGet(ResourceType.Euro, 10);

        actualResult.Should().BeFalse();
    }

    [Fact]
    public void TryGet_WithJustEnoughInRepository_ShouldReturnSuccess()
    {
        this.repositoryUnderTest.Store(ResourceType.Euro, 10);

        var actualResult = this.repositoryUnderTest.TryGet(ResourceType.Euro, 10);

        actualResult.Should().BeTrue();

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Euro, -10, 0),
            Times.Once);
    }

    [Fact]
    public void TryGet_WithMoreThanEnoughInRepository_ShouldReturnSuccess()
    {
        this.repositoryUnderTest.Store(ResourceType.Euro, 20);

        var actualResult = this.repositoryUnderTest.TryGet(ResourceType.Euro, 10);

        actualResult.Should().BeTrue();

        this.presentationServiceMock.Verify(p =>
                p.UpdateResourceBalance(ResourceType.Euro, -10, 10),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void TryGet_WithInvalidAmount_ShouldThrow(int invalidAmount)
    {
        this.repositoryUnderTest
            .Invoking(r => r.TryGet(ResourceType.Bar, invalidAmount))
            .Should().Throw<ResourceRepositoryRequestAmountException>();
    }

    [Fact]
    public void TryGetRange_WithEmptyRepositoryAndMinimumNotZero_ShouldReturnZero()
    {
        var valueRetrieved = this.repositoryUnderTest.TryGetRange(ResourceType.Foobar, 1, 5);

        valueRetrieved.Should().Be(0);
    }

    [Fact]
    public void TryGetRange_WithEmptyRepositoryForResourceAndMinimumNotZero_ShouldReturnZero()
    {
        this.repositoryUnderTest.Store(ResourceType.Euro, 19);

        var valueRetrieved = this.repositoryUnderTest.TryGetRange(ResourceType.Foobar, 1, 5);

        valueRetrieved.Should().Be(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void TryGetRange_WithTotalRepositoryAmountInRange_ShouldReturnEntireAmount(int repositoryAmount)
    {
        this.repositoryUnderTest.Store(ResourceType.Foobar, repositoryAmount);

        var valueRetrieved = this.repositoryUnderTest.TryGetRange(ResourceType.Foobar, 1, 5);

        valueRetrieved.Should().Be(repositoryAmount);

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Foobar, -repositoryAmount, 0),
            Times.Once);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(int.MaxValue)]
    public void TryGetRange_WithTotalRepositoryAmountMoreThanRange_ShouldReturnMaxRequestedAmount(int repositoryAmount)
    {
        this.repositoryUnderTest.Store(ResourceType.Foobar, repositoryAmount);

        var valueRetrieved = this.repositoryUnderTest.TryGetRange(ResourceType.Foobar, 1, 5);

        valueRetrieved.Should().Be(5);

        this.presentationServiceMock.Verify(p =>
            p.UpdateResourceBalance(ResourceType.Foobar, -5, repositoryAmount - 5),
            Times.Once);
    }

    [Theory]
    [InlineData(5, 3)]
    [InlineData(4, 4)]
    public void TryGetRange_WithInvalidRange_ShouldThrow(int minimum, int maximum)
    {
        this.repositoryUnderTest
            .Invoking(r => r.TryGetRange(ResourceType.Foobar, minimum, maximum))
            .Should().Throw<ResourceRepositoryRequestAmountRangeException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(int.MinValue)]
    public void TryGetRange_WithInvalidMinimum_ShouldThrow(int minimum)
    {
        this.repositoryUnderTest
            .Invoking(r => r.TryGetRange(ResourceType.Foobar, minimum, 10))
            .Should().Throw<ResourceRepositoryRequestAmountException>();
    }
}
using FoobarFactoryDemo.Application.Activities.Domain;
using FoobarFactoryDemo.Application.Activities.Models;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Domain;

[Trait("Category", "UnitTests")]
public class ChangingActivityTests
{
    private readonly IActivity activityUnderTest;

    public ChangingActivityTests()
    {
        this.activityUnderTest = new ChangingActivity();
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
    }
}
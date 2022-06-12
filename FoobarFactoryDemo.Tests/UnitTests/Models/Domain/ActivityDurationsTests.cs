using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Models.Domain;

[Trait("Category", "UnitTests")]
public class ActivityDurationsTests
{
    private readonly ActivityDurations durationsUnderTest;

    public ActivityDurationsTests()
    {
        var randomGeneratorMock = new Mock<IRandomGenerator>();
        randomGeneratorMock
            .Setup(m => m.NextDouble())
            .Returns(0.1);

        this.durationsUnderTest = new ActivityDurations(
            TimeSpan.FromSeconds(1),
            new VariableTimeSpan(randomGeneratorMock.Object, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3)),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(6),
            TimeSpan.FromSeconds(7));
    }

    [Theory]
    [InlineData(ActivityType.AssemblingFoobar)]
    [InlineData(ActivityType.BuyingRobot)]
    [InlineData(ActivityType.ChangingActivity)]
    [InlineData(ActivityType.MiningBar)]
    [InlineData(ActivityType.MiningFoo)]
    [InlineData(ActivityType.SellingFoobar)]
    public void GetDuration_WithValidActivity_ReturnsTimeSpan(ActivityType validActivity)
    {
        var timeSpan = this.durationsUnderTest.GetDuration(validActivity);
        timeSpan.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Theory]
    [InlineData(ActivityType.Idling)]
    [InlineData((ActivityType)123123)]
    public void GetDuration_WithUnsupportedActivity_ShouldThrow(ActivityType invalidActivity)
    {
        this.durationsUnderTest
            .Invoking(d => d.GetDuration(invalidActivity))
            .Should().Throw<NotSupportedException>();
    }
}
using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Tests.UnitTests.Models.Domain;

[Trait("Category", "UnitTests")]
public class ActivityWeightsTests
{
    private readonly ActivityWeights weightsUnderTest;

    public ActivityWeightsTests()
    {
        this.weightsUnderTest = new ActivityWeights(
            new WeightRange(0, 0.2),
            new WeightRange(0.2, 0.4),
            new WeightRange(0.4, 0.6),
            new WeightRange(0.6, 0.8),
            new WeightRange(0.8, 1),
            new WeightRange(0, 0.5));
    }

    [Fact]
    public void GetActionableActivity_WithinRange_ReturnsActivity()
    {
        for (var i = 0.0; i < 1.0; i += 0.01)
        {
            var activity = this.weightsUnderTest.GetActionableActivity(i);

            activity.Should().BeOneOf(
                ActivityType.AssemblingFoobar,
                ActivityType.BuyingRobot,
                ActivityType.MiningBar,
                ActivityType.MiningFoo,
                ActivityType.SellingFoobar);
        }
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(-0.0000001)]
    [InlineData(-1)]
    [InlineData(double.MinValue)]
    public void GetActionableActivity_OutsideOfRange_ShouldThrow(double badValue)
    {
        this.weightsUnderTest
            .Invoking(w => w.GetActionableActivity(badValue))
            .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetChangeableActivity_WithinRange_ReturnsActivity()
    {
        const ActivityType previousValue = ActivityType.BuyingRobot;

        for (var i = 0.0; i < 1.0; i += 0.01)
        {
            var activity = this.weightsUnderTest.GetChangeableActivity(i, previousValue);

            activity.Should().BeOneOf(ActivityType.ChangingActivity, previousValue);
        }
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(-0.0000001)]
    [InlineData(-1)]
    [InlineData(double.MinValue)]
    public void GetChangeableActivity_OutsideOfRange_ShouldThrow(double badValue)
    {
        this.weightsUnderTest
            .Invoking(w => w.GetChangeableActivity(badValue, ActivityType.AssemblingFoobar))
            .Should().Throw<ArgumentOutOfRangeException>();
    }
}
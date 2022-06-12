using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Infrastructure.Exceptions;
using FoobarFactoryDemo.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Factories;

[Trait("Category", "UnitTests")]
public class ActivityWeightsFactoryTests
{
    [Fact]
    public void Construction_WithNullSettingsOptions_ShouldThrow()
    {
        var construction = () => new ActivityWeightsFactory(
            activityPickerSettings: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityPickerSettings");
    }

    [Fact]
    public void Construction_WithNullSettings_ShouldThrow()
    {
        var construction = () => new ActivityWeightsFactory(
            activityPickerSettings: Options.Create<WeightedActivityPickerSettings>(null!));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityPickerSettings");
    }

    [Fact]
    public void CreateActivityWeights_WithValidData_ShouldReturnValidWeights()
    {
        for (var weight = 0.01; weight < 1; weight += 0.01)
        {
            var factoryUnderTest = new ActivityWeightsFactory(Options.Create(
                new WeightedActivityPickerSettings
                {
                    AssemblingFoobarWeight = weight,
                    BuyingRobotWeight = weight,
                    ChangingActivityWeight = weight,
                    MiningBarWeight = weight,
                    MiningFooWeight = weight,
                    SellingFoobarWeight = weight
                }));

            var weights = factoryUnderTest.CreateActivityWeights();

            weights.Should().NotBeNull();
        }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-0.001)]
    [InlineData(-1)]
    public void CreateActivityWeights_WithInvalidChangingActivityWeights_ShouldThrow(double changeActivityWeight)
    {
        var factoryUnderTest = new ActivityWeightsFactory(Options.Create(
            new WeightedActivityPickerSettings
            {
                AssemblingFoobarWeight = 0.2,
                BuyingRobotWeight = 0.2,
                ChangingActivityWeight = changeActivityWeight,
                MiningBarWeight = 0.2,
                MiningFooWeight = 0.2,
                SellingFoobarWeight = 0.2
            }));

        factoryUnderTest
            .Invoking(f => f.CreateActivityWeights())
            .Should().Throw<ChangingActivityWeightOutOfRangeException>();
    }

    [Fact]
    public void CreateActivityWeights_WithInvalidActionableActivityWeights_ShouldThrow()
    {
        var factoryUnderTest = new ActivityWeightsFactory(Options.Create(
            new WeightedActivityPickerSettings
            {
                AssemblingFoobarWeight = 0,
                BuyingRobotWeight = 0,
                ChangingActivityWeight = 0.5,
                MiningBarWeight = 0,
                MiningFooWeight = 0,
                SellingFoobarWeight = 0
            }));

        factoryUnderTest
            .Invoking(f => f.CreateActivityWeights())
            .Should().Throw<ActionableActivityWeightOutOfRangeException>();
    }
}
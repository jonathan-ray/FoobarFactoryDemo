using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Infrastructure.Settings;
using FoobarFactoryDemo.Utilities;
using Microsoft.Extensions.Options;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities.Factories;

[Trait("Category", "UnitTests")]
public class ActivityDurationsFactoryTests
{
    private static readonly ActivityDurationSettings DefaultSettings = new()
    {
        AssemblingFoobarDuration = 10,
        BuyingRobotDuration = 20,
        ChangingActivityDuration = 30,
        MiningBarDurationRangeStart = 40,
        MiningBarDurationRangeEnd = 50,
        MiningFooDuration = 60,
        SellingFoobarDuration = 70,
        InverseDurationCoefficient = 100
    };

    private readonly IActivityDurationsFactory factoryUnderTest;

    public ActivityDurationsFactoryTests()
    {
        this.factoryUnderTest = new ActivityDurationsFactory(
            Mock.Of<IRandomGenerator>(),
            Options.Create(DefaultSettings));
    }

    [Fact]
    public void Construction_WithNullRandomGenerator_ShouldThrow()
    {
        var construction = () => new ActivityDurationsFactory(
            randomGenerator: null!,
            Options.Create(DefaultSettings));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("randomGenerator");
    }

    [Fact]
    public void Construction_WithNullSettingsOption_ShouldThrow()
    {
        var construction = () => new ActivityDurationsFactory(
            Mock.Of<IRandomGenerator>(),
            activityDurationSettings: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityDurationSettings");
    }

    [Fact]
    public void Construction_WithNullSettings_ShouldThrow()
    {
        var construction = () => new ActivityDurationsFactory(
            Mock.Of<IRandomGenerator>(),
            Options.Create<ActivityDurationSettings>(null!));

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityDurationSettings");
    }

    [Fact]
    public void CreateDurations_WithValidData_ShouldReturnValidDurations()
    {
        var durations = this.factoryUnderTest.CreateDurations();

        durations.Should().NotBeNull();
        durations.ChangingActivity.Should().BeGreaterThan(TimeSpan.Zero);
        durations.AssemblingFoobar.Should().BeGreaterThan(TimeSpan.Zero);
        durations.BuyingRobot.Should().BeGreaterThan(TimeSpan.Zero);
        durations.MiningBar.NextValue.Should().BeGreaterThan(TimeSpan.Zero);
        durations.MiningFoo.Should().BeGreaterThan(TimeSpan.Zero);
        durations.SellingFoobar.Should().BeGreaterThan(TimeSpan.Zero);
    }
}
using FoobarFactoryDemo.Application.Activities;
using FoobarFactoryDemo.Application.Activities.Factories;
using FoobarFactoryDemo.Models.Domain;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Application.Activities;

[Trait("Category", "UnitTests")]
public class WeightedActivityPickerTests
{
    private readonly Mock<IRandomGenerator> randomGeneratorMock;
    private readonly Mock<IActivityWeights> weightsMock;


    private readonly IActivityPicker pickerUnderTest;

    public WeightedActivityPickerTests()
    {
        this.randomGeneratorMock = new Mock<IRandomGenerator>();
        this.weightsMock = new Mock<IActivityWeights>();
        var weightsFactoryMock = new Mock<IActivityWeightsFactory>();
        weightsFactoryMock
            .Setup(m => m.CreateActivityWeights())
            .Returns(this.weightsMock.Object);

        this.pickerUnderTest = new WeightedActivityPicker(
            this.randomGeneratorMock.Object,
            weightsFactoryMock.Object);
    }

    [Fact]
    public void Construction_WithNullRandomGenerator_ShouldThrow()
    {
        var construction = () => new WeightedActivityPicker(
            randomGenerator: null!,
            Mock.Of<IActivityWeightsFactory>());

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("randomGenerator");
    }

    [Fact]
    public void Construction_WithNullActivityWeightsFactory_ShouldThrow()
    {
        var construction = () => new WeightedActivityPicker(
            Mock.Of<IRandomGenerator>(),
            activityWeightsFactory: null!);

        construction
            .Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("activityWeightsFactory");
    }

    [Fact]
    public void GetNextActivity_WithIdling_ShouldReturnChangingActivity()
    {
        var nextActivity = this.pickerUnderTest.GetNextActivity(ActivityType.Idling);

        nextActivity.Should().Be(ActivityType.ChangingActivity);
    }

    [Theory]
    [InlineData(ActivityType.AssemblingFoobar)]
    [InlineData(ActivityType.BuyingRobot)]
    [InlineData(ActivityType.MiningBar)]
    [InlineData(ActivityType.MiningFoo)]
    [InlineData(ActivityType.SellingFoobar)]
    public void GetNextActivity_WithActionableActivity_ShouldGetChangeableActivity(ActivityType activity)
    {
        const double randomValue = 0.5;

        this.randomGeneratorMock
            .Setup(m => m.NextDouble())
            .Returns(randomValue);

        var expectedNextActivity = ActivityType.ChangingActivity;

        this.weightsMock
            .Setup(m => m.GetChangeableActivity(randomValue, activity))
            .Returns(expectedNextActivity);

        var actualNextActivity = this.pickerUnderTest.GetNextActivity(activity);

        actualNextActivity.Should().Be(expectedNextActivity);

        this.weightsMock.Verify(m =>
            m.GetChangeableActivity(randomValue, activity),
            Times.Once);
    }

    [Fact]
    public void GetNextActivity_WithChangingActivity_ShouldGetChangeableActivity()
    {
        const double randomValue = 0.5;

        this.randomGeneratorMock
            .Setup(m => m.NextDouble())
            .Returns(randomValue);

        var expectedNextActivity = ActivityType.AssemblingFoobar;

        this.weightsMock
            .Setup(m => m.GetActionableActivity(randomValue))
            .Returns(expectedNextActivity);

        var actualNextActivity = this.pickerUnderTest.GetNextActivity(ActivityType.ChangingActivity);

        actualNextActivity.Should().Be(expectedNextActivity);

        this.weightsMock.Verify(m =>
            m.GetActionableActivity(randomValue),
            Times.Once);
    }
}
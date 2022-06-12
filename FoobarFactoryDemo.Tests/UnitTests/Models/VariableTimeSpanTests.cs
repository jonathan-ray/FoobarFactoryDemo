using FoobarFactoryDemo.Models;
using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Models;

[Trait("Category", "UnitTests")]
public class VariableTimeSpanTests
{
    private readonly Mock<IRandomGenerator> randomGeneratorMock;

    public VariableTimeSpanTests()
    {
        this.randomGeneratorMock = new Mock<IRandomGenerator>();
    }

    [Fact]
    public void NextValue_WithValidRandom_ShouldReturnDifferentValueAndInRange()
    {
        var start = TimeSpan.FromSeconds(5);
        var end = TimeSpan.FromSeconds(10);

        var variableTimeSpan = new VariableTimeSpan(this.randomGeneratorMock.Object, start, end);

        var lastValue = TimeSpan.Zero;

        for (var i = 0.0; i < 1.0; i += 0.01)
        {
            this.randomGeneratorMock
                .Setup(m => m.NextDouble())
                .Returns(i);

            var actualValue = variableTimeSpan.NextValue;

            actualValue.Should().BeGreaterThanOrEqualTo(start);
            actualValue.Should().BeLessThan(end);

            actualValue.Should().NotBe(lastValue);

            lastValue = actualValue;
        }
    }
}
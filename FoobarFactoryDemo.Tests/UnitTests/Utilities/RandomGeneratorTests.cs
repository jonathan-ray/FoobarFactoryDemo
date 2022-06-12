using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Tests.UnitTests.Utilities;

[Trait("Category", "UnitTests")]
public class RandomGeneratorTests
{
    private readonly IRandomGenerator generatorUnderTest;

    public RandomGeneratorTests()
    {
        this.generatorUnderTest = new RandomGenerator();
    }

    [Fact]
    public void NextDouble_ShouldReturnDifferentValueInCorrectRange()
    {
        var lastValue = -1.0;
        for (var i = 0; i < 100; i++)
        {
            var currentValue = this.generatorUnderTest.NextDouble();

            currentValue.Should().BeGreaterOrEqualTo(0.0);
            currentValue.Should().BeLessThan(1.0);

            currentValue.Should().NotBe(lastValue);

            lastValue = currentValue;
        }
    }
}
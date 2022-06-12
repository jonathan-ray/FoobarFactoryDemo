namespace FoobarFactoryDemo.Utilities;

public class RandomGenerator : IRandomGenerator
{
    private readonly Random randomGenerator = new();

    public double NextDouble()
    {
        return this.randomGenerator.NextDouble();
    }
}
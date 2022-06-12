namespace FoobarFactoryDemo.Utilities;

/// <summary>
/// Abstraction of generating random numbers.
/// </summary>
public interface IRandomGenerator
{
    /// <summary>
    /// Retrieves a new random double in the range of 0 to 1.
    /// </summary>
    /// <returns>A random double value.</returns>
    double NextDouble();
}
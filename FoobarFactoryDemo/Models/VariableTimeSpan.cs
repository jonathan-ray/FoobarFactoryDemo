using FoobarFactoryDemo.Utilities;

namespace FoobarFactoryDemo.Models;

/// <summary>
/// A time span that randomly returns a value within a specific range.
/// </summary>
public class VariableTimeSpan
{
    private readonly IRandomGenerator randomGenerator;
    private readonly TimeSpan start;
    private readonly TimeSpan delta;

    public VariableTimeSpan(IRandomGenerator randomGenerator, TimeSpan start, TimeSpan end)
    {
        this.randomGenerator = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));

        if (end <= start)
        {
            throw new ArgumentOutOfRangeException(nameof(end));
        }

        this.start = start;
        this.delta = end - start;
    }

    /// <summary>
    /// Retrieves a random timespan value within the pre-defined variable range.
    /// </summary>
    public TimeSpan NextValue => this.start + (this.randomGenerator.NextDouble() * this.delta);
}
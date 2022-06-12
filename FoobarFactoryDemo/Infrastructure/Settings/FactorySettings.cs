namespace FoobarFactoryDemo.Infrastructure.Settings;

/// <summary>
/// General Foobar Factory settings.
/// </summary>
public class FactorySettings
{
    /// <summary>
    /// Defines the amount of workers to begin with.
    /// </summary>
    public int InitialWorkerCount { get; init; }

    /// <summary>
    /// Defines the amount of workers that should trigger an automatic shutdown.
    /// </summary>
    public int ShutdownOnWorkerCount { get; init; }
}
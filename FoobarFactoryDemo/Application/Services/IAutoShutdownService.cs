namespace FoobarFactoryDemo.Application.Services;

/// <summary>
/// Abstraction of the conditions for automatically shutting down the application.
/// </summary>
public interface IAutoShutdownService
{
    /// <summary>
    /// Defines whether the application should shut down.
    /// </summary>
    bool ShouldShutdown { get; }
}
using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Presentation;

/// <summary>
/// Service to update the presentation layer on general topics.
/// </summary>
public interface IPresentationService
{
    /// <summary>
    /// Pause the output.
    /// </summary>
    Task Pause();

    /// <summary>
    /// Declare the scheduler has started.
    /// </summary>
    void SchedulerStarted();

    /// <summary>
    /// Declare the scheduler has ended.
    /// </summary>
    void SchedulerEnded();

    /// <summary>
    /// Declare that the factory is shutting down.
    /// </summary>
    void ShutdownInitiated();

    /// <summary>
    /// Declare a resource's balance has been updated.
    /// </summary>
    /// <param name="resource">The resource's type.</param>
    /// <param name="delta">The delta (+/-) being made to the balance.</param>
    /// <param name="resultingBalance">The resulting balance of the update.</param>
    void UpdateResourceBalance(ResourceType resource, int delta, int resultingBalance);

    /// <summary>
    /// Display a welcome message.
    /// </summary>
    void WelcomeMessage();
}
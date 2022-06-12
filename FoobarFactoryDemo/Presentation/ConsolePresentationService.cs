using FoobarFactoryDemo.Models.Domain;

namespace FoobarFactoryDemo.Presentation;

public class ConsolePresentationService : IPresentationService, IWorkerPresentationService<RobotState>
{
    public void ActivityStarted(RobotState workerState)
    {
        Output(workerState.Id, $"Started activity {workerState.Activity}");
    }

    public void ActivityCompleted(RobotState workerState)
    {
        Output(workerState.Id, $"Completed activity {workerState.Activity}");
    }

    public void ActivityFailed(RobotState workerState)
    {
        Output(workerState.Id, $"Failed to complete activity {workerState.Activity}");
    }

    public void WorkerAdded(int totalWorkers)
    {
        Output($"New worker added! Total workers: {totalWorkers}.");
    }

    public Task Pause()
    {
        Console.ReadKey();
        return Task.CompletedTask;
    }

    public void SchedulerStarted()
    {
        Output("Scheduler started.");
    }

    public void SchedulerEnded()
    {
        Output("Scheduler ended.");
    }

    public void ShutdownInitiated()
    {
        Output("Conditions have been met to shut down the factory. Finishing all remaining activities.");
    }

    public void UpdateResourceBalance(ResourceType resource, int delta, int resultingBalance)
    {
        if (delta == 0)
        {
            Output($"No changes made to {resource} balance.");
        }
        else
        {
            var amount = delta > 0
                ? $"+{delta}"
                : delta.ToString();

            Output($"Updated {resource} balance: {resultingBalance} ({amount}).");

        }
    }

    public void WelcomeMessage()
    {
        Output("Welcome to the Foobar Factory! Press any key to run:");
    }

    private static void Output(int workerId, string message)
    {
        Output($"Worker {workerId}: {message}");
    }

    private static void Output(string message)
    {
        Console.WriteLine(message);
    }
}
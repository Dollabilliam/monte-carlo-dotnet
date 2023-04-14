namespace MonteCarloSimulator.Options;

public record QueueWorkerOptions
{
    public int? WorkerDelayInMs { get; init; }
}
namespace MonteCarloSimulator.Options;

public record QueueWorkerOptions
{
    public int? WorkerDelay { get; init; }
}
namespace MonteCarloSimulator.Options;

public record ScenarioProcessorOptions
{
    public int? NumberOfHostProcessors { get; init; }
};
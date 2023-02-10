using MonteCarloSimulator.ScenarioProcessor;

namespace MonteCarloSimulator.Queues.Messages;

public record QueueMessage
{
    public SimulationObject SimulationObject { get; init; }
}
using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator.ScenarioProcessor;

public interface IScenarioProcessor
{
    public Task ProcessScenarios(QueueMessage message, CancellationToken cancellationToken);
}
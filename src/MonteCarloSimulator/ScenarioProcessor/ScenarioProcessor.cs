using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator.ScenarioProcessor;

public class ScenarioProcessor : IScenarioProcessor
{

    private List<double> AssetsScenarioReturns = new();

    private readonly ILogger<ScenarioProcessor> logger;

    public ScenarioProcessor(ILogger<ScenarioProcessor> logger)
    {
        this.logger = logger;
    }

    public async Task ProcessScenarios(QueueMessage message, CancellationToken cancellationToken)
    {
    }
}
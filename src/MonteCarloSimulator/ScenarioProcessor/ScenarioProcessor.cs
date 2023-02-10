using System.Threading.Tasks.Dataflow;
using MathNet.Numerics.Distributions;
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
        logger.LogInformation("Beginning processing scenarios for asset {AssetSimId}", message.SimulationObject.SimulationId);

        var actions = new ActionBlock<SimulationObject>(
            _ => ProcessSimulation(message.SimulationObject),
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 10,
                CancellationToken = cancellationToken
            });

        for (var scenario = 0; scenario < message.SimulationObject.Scenarios; scenario++)
        {
            actions.Post(message.SimulationObject);
        }
        actions.Complete();
        await actions.Completion;
    }

    private void ProcessSimulation(SimulationObject simulationObject)
    {

    }
}
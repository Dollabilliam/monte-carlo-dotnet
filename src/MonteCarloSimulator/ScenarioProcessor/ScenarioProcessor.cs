using System.Threading.Tasks.Dataflow;
using MathNet.Numerics.Distributions;
using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.Result;
using MonteCarloSimulator.Status;

namespace MonteCarloSimulator.ScenarioProcessor;

public class ScenarioProcessor : IScenarioProcessor
{

    private List<double> AssetsScenarioReturns = new();

    private readonly ILogger<ScenarioProcessor> logger;
    private readonly ISetStatusRepository statusRepository;
    private readonly ISetResultRepository resultRepository;

    public ScenarioProcessor(ILogger<ScenarioProcessor> logger, ISetStatusRepository statusRepository, ISetResultRepository resultRepository)
    {
        this.logger = logger;
        this.statusRepository = statusRepository;
        this.resultRepository = resultRepository;
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
            await statusRepository.SetStatus(message.SimulationObject.SimulationId, scenario);

            logger.LogInformation(
                "Beginning scenario {Scenario} for asset {AssetSimId}",
                message.SimulationObject.SimulationId,
                scenario);

            actions.Post(message.SimulationObject);
        }

        actions.Complete();
        await actions.Completion;

        logger.LogInformation("Setting scenario result for asset {AssetSimId}", message.SimulationObject.SimulationId);

        var quantiles = CalculateQuantiles(AssetsScenarioReturns);
        await resultRepository.SetResult(message.SimulationObject.SimulationId, quantiles);
    }

    private void ProcessSimulation(SimulationObject simulationObject)
    {
        var price = simulationObject.InitialPrice;
        for (var timestep = 0; timestep < simulationObject.TimeSteps; timestep++)
        {
            var normalDistribution = new Normal(simulationObject.Mean, simulationObject.StandardDeviation);
            var randomNumber = normalDistribution.Sample();
            var exp = Math.Exp(randomNumber);

            price *= exp;
        }

        var simulationReturn = price / simulationObject.InitialPrice - 1;
        AssetsScenarioReturns.Add(simulationReturn);
    }

    private Quantiles CalculateQuantiles(List<double> scenarioReturns)
    {
        scenarioReturns.Sort();

        var quantileSize = scenarioReturns.Count / 5;

        var firstQuantile = scenarioReturns[quantileSize - 1];
        var fifthQuantile = scenarioReturns[4 * quantileSize - 1];

        return new Quantiles
        {
            FirstQuantileReturn = firstQuantile,
            FifthQuantileReturn = fifthQuantile
        };
    }
}
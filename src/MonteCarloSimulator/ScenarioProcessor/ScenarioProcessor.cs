using System.Threading.Tasks.Dataflow;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Options;
using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Options;
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
    private readonly IOptions<ScenarioProcessorOptions> options;

    public ScenarioProcessor(ILogger<ScenarioProcessor> logger, ISetStatusRepository statusRepository, ISetResultRepository resultRepository, IOptions<ScenarioProcessorOptions> options)
    {
        this.logger = logger;
        this.statusRepository = statusRepository;
        this.resultRepository = resultRepository;
        this.options = options;
    }

    public async Task ProcessScenarios(QueueMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Beginning processing scenarios for asset {AssetSimId}", message.SimulationObject.SimulationId);

        var actionBlock = new ActionBlock<SimulationObject>(
           async _ => await ProcessSimulation(
                message.SimulationObject.InitialPrice,
                message.SimulationObject.TimeSteps,
                message.SimulationObject.Mean,
                message.SimulationObject.StandardDeviation),
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = options.Value.NumberOfHostProcessors ?? Environment.ProcessorCount,
                CancellationToken = cancellationToken
            });

        for (var scenario = 0; scenario < message.SimulationObject.Scenarios; scenario++)
        {
            statusRepository.SetStatus(message.SimulationObject.SimulationId, scenario);

            logger.LogInformation(
                "Beginning scenario {Scenario} for asset {AssetSimId}",
                message.SimulationObject.SimulationId,
                scenario);

            await actionBlock.SendAsync(message.SimulationObject, cancellationToken);
        }

        actionBlock.Complete();
        await actionBlock.Completion;

        logger.LogInformation("Setting scenario result for asset {AssetSimId}", message.SimulationObject.SimulationId);

        var quantiles = CalculateQuantiles(AssetsScenarioReturns);
        await resultRepository.SetResult(message.SimulationObject.SimulationId, quantiles);
    }

    private Task ProcessSimulation(double initialPrice, int timeSteps, double mean, double standardDeviation)
    {
        var simulatedPrice = initialPrice;
        for (var timeStep = 0; timeStep < timeSteps; timeStep++)
        {
            var normalDistribution = new Normal(mean, standardDeviation);
            var randomNumber = normalDistribution.Sample();
            var exp = Math.Exp(randomNumber);

            simulatedPrice *= exp;
        }

        var simulationReturn = simulatedPrice / initialPrice - 1;
        AssetsScenarioReturns.Add(simulationReturn);

        return Task.CompletedTask;
    }

    // ToDo: return formatted bell curve or boxplot of scenario results too -- would be way cooler!

    private Quantiles CalculateQuantiles(List<double> scenarioReturns)
    {
        scenarioReturns.Sort();

        var quantileSize = scenarioReturns.Count / 5;

        var firstQuantile = scenarioReturns[quantileSize - 1];
        var fifthQuantile = scenarioReturns[4 * quantileSize - 1];

        return new Quantiles
        {
            FirstQuantileReturnAsPercentage = firstQuantile,
            FifthQuantileReturnAsPercentage = fifthQuantile
        };
    }
}
using Microsoft.AspNetCore.Mvc;
using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Controllers.Dtos.Requests;
using MonteCarloSimulator.Controllers.Dtos.Responses;
using MonteCarloSimulator.Queues;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.ScenarioProcessor;

namespace MonteCarloSimulator.Controllers;

[ApiController]
[Route("simulation")]
public class SimulationController : ControllerBase
{
    private readonly IEnqueueRequest<QueueMessage> queue;

    public SimulationController(IEnqueueRequest<QueueMessage> queue)
    {
        this.queue = queue;
    }

    [HttpPost("start-simulation")]
    [ProducesResponseType(typeof(PostSimulationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostSimulationResponseDto>> StartSimulation(PostSimulationRequestDto request)
    {
        var simulationIds = new List<string>();
        foreach (var asset in request.Assets)
        {
            try
            {
                var simulationId = $"{Guid.NewGuid()}_{asset.Name}";

                var message = CreateMessage(simulationId, request.Scenarios, request.TimeSteps, asset);

                await queue.Enqueue(message);

                simulationIds.Add(simulationId);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        return new PostSimulationResponseDto { SimulationIds = simulationIds };
    }

    private QueueMessage CreateMessage(string simulationId, int scenarios, int timeSteps, Asset asset)
    {
        var meanAsDouble = ConvertPercentageToDouble(asset.Mean);
        var stdDevAsDouble = ConvertPercentageToDouble(asset.StandardDeviation);

        if (meanAsDouble == 0 | stdDevAsDouble == 0)
        {
            throw new Exception("Invalid mean and standard deviation was provided. Valid examples include '0.01%' and '5%'");
        }

        return new QueueMessage
        {
            SimulationObject = new SimulationObject
            {
                SimulationId = simulationId,
                Scenarios = scenarios,
                TimeSteps = timeSteps,
                InitialPrice = asset.InitialPrice,
                Mean = meanAsDouble,
                StandardDeviation = stdDevAsDouble
            }
        };
    }

    private double ConvertPercentageToDouble(string percentage)
    {
        if (double.TryParse(percentage.TrimEnd('%'), out double result))
        {
            return result / 100;
        }

        return 0;
    }
}
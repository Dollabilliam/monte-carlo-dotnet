using Microsoft.AspNetCore.Mvc;
using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Controllers.Dtos.Requests;
using MonteCarloSimulator.Controllers.Dtos.Responses;
using MonteCarloSimulator.Helpers;
using MonteCarloSimulator.Queues;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.Result;
using MonteCarloSimulator.ScenarioProcessor;
using MonteCarloSimulator.Status;

namespace MonteCarloSimulator.Controllers;

[ApiController]
[Route("simulation")]
public class SimulationController : ControllerBase
{
    private readonly IEnqueueQueue<QueueMessage> queue;
    private readonly IGetStatusRepository statusRepository;
    private readonly IGetResultRepository resultRepository;

    public SimulationController(
        IEnqueueQueue<QueueMessage> queue,
        IGetStatusRepository statusRepository,
        IGetResultRepository resultRepository)
    {
        this.queue = queue;
        this.statusRepository = statusRepository;
        this.resultRepository = resultRepository;
    }

    [HttpPost("start-simulation")]
    [ProducesResponseType(typeof(PostSimulationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult StartSimulation(PostSimulationRequestDto request)
    {
        var simulationIds = new List<string>();
        foreach (var asset in request.Assets)
        {
            try
            {
                var simulationId = $"{Guid.NewGuid()}_{asset.Name}";

                var message = MessageHelper.CreateMessage(simulationId, request.Scenarios, request.TimeSteps, asset);

                queue.Enqueue(message);

                simulationIds.Add(simulationId);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        return Ok(new PostSimulationResponseDto { SimulationIds = simulationIds });
    }

    [HttpGet("get-statuses")]
    [ProducesResponseType(typeof(GetStatusResponseDto), StatusCodes.Status200OK)]
    public ActionResult GetStatuses([FromQuery] GetStatusRequestDto request)
    {
        return Ok(statusRepository.GetStatus(request.SimulationIds));
    }


    [HttpGet("get-results")]
    [ProducesResponseType(typeof(GetResultResponseDto), StatusCodes.Status200OK)]
    public ActionResult GetResults([FromQuery] GetResultRequestDto request)
    {
        return Ok(resultRepository.GetResults(request.SimulationIds));
    }
}
namespace MonteCarloSimulator.Controllers.Dtos.Responses;

public record GetStatusResponseDto
{
    public string SimulationId { get; init; }

    public int ScenarioCount { get; init; }
}
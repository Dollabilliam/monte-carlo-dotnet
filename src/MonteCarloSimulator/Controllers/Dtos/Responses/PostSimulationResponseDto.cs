namespace MonteCarloSimulator.Controllers.Dtos.Responses;

public record PostSimulationResponseDto
{
    public IEnumerable<string> SimulationIds { get; init; }
}
namespace MonteCarloSimulator.Controllers.Dtos.Requests;

public record GetResultRequestDto
{
    public IEnumerable<string> SimulationIds { get; init; }
}
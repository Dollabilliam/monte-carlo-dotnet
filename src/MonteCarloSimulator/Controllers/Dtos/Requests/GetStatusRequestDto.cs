namespace MonteCarloSimulator.Controllers.Dtos.Requests;

public record GetStatusRequestDto
{
    public IEnumerable<string> SimulationIds { get; init; }
}
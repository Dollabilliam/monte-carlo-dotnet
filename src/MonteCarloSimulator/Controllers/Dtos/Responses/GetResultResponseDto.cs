namespace MonteCarloSimulator.Controllers.Dtos.Responses;

public record GetResultResponseDto
{
    public string SimulationId { get; init; }

    public Quantiles Quantiles { get; init; }
}
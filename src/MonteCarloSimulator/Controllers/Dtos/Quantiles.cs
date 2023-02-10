namespace MonteCarloSimulator.Controllers.Dtos;

public record Quantiles
{
    public double FirstQuantileReturn { get; init; }

    public double FifthQuantileReturn { get; init; }
}
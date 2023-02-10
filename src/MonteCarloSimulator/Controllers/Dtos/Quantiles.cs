namespace MonteCarloSimulator.Controllers.Dtos;

public record Quantiles
{
    public double FirstQuantileReturnAsPercentage { get; init; }

    public double FifthQuantileReturnAsPercentage { get; init; }
}
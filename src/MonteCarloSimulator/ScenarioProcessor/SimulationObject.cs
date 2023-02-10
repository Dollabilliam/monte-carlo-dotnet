namespace MonteCarloSimulator.ScenarioProcessor;

public record SimulationObject
{
    public string SimulationId { get; init; }

    public int Scenarios { get; init; }

    public int TimeSteps { get; init; }

    public double InitialPrice { get; init; }

    public double Mean { get; init; }

    public double StandardDeviation { get; init; }
}
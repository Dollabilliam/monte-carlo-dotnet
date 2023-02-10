namespace MonteCarloSimulator.Status;

public interface ISetStatusRepository
{
    public Task SetStatus(string simulationId, int currentScenario);
}
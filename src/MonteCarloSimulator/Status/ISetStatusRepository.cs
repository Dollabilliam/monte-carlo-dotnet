namespace MonteCarloSimulator.Status;

public interface ISetStatusRepository
{
    public void SetStatus(string simulationId, int currentScenario);
}
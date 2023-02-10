using MonteCarloSimulator.Controllers.Dtos;

namespace MonteCarloSimulator.Result;

public interface ISetResultRepository
{
    public Task SetResult(string simulationId, Quantiles quantiles);
}
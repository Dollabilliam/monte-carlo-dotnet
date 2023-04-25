using System.Collections.Concurrent;
using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Result;

public class ResultRepository : IGetResultRepository, ISetResultRepository
{
    private static ConcurrentDictionary<string, Quantiles> ResultDictionary = new();

    public Task SetResult(string simulationId, Quantiles quantiles)
    {
        ResultDictionary[simulationId] = quantiles;
        return Task.CompletedTask;
    }

    public IEnumerable<GetResultResponseDto> GetResults(IEnumerable<string> simulationIds)
    {
        var getResultResponseDtos = new List<GetResultResponseDto>();
        foreach (var simulationId in simulationIds)
        {
            if (ResultDictionary.TryGetValue(simulationId, out var quantiles))
            {
                getResultResponseDtos.Add(new GetResultResponseDto
                {
                    SimulationId = simulationId,
                    Quantiles = quantiles
                });
            }
        }

        return getResultResponseDtos;
    }
}
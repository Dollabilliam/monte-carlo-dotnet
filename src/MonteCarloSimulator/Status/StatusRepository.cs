using System.Collections.Concurrent;
using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Status;

public class StatusRepository : IGetStatusRepository
{
    private static ConcurrentDictionary<string, int> StatusDictionary = new();

    public Task<List<GetStatusResponseDto>> GetStatus(IEnumerable<string> simulationIds)
    {
        var getStatusResponseDtos = new List<GetStatusResponseDto>();

        foreach (var simulationId in simulationIds)
        {
            if (StatusDictionary.TryGetValue(simulationId, out var currentScenario))
            {
                getStatusResponseDtos.Add(new GetStatusResponseDto
                {
                    SimulationId = simulationId,
                    ScenarioCount = currentScenario
                });
            }
        }

        return Task.FromResult(getStatusResponseDtos);
    }
}
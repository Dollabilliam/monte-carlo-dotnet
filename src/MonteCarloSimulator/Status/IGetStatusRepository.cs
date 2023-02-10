using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Status;

public interface IGetStatusRepository
{
    Task<List<GetStatusResponseDto>> GetStatus(IEnumerable<string> simulationIds);
}
using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Status;

public interface IGetStatusRepository
{
    IEnumerable<GetStatusResponseDto> GetStatus(IEnumerable<string> simulationIds);
}
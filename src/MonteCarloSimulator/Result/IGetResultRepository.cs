using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Result;

public interface IGetResultRepository
{
    public Task<List<GetResultResponseDto>> GetResults(IEnumerable<string> simulationIds);
}
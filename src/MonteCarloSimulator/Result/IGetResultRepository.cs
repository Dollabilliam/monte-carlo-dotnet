using MonteCarloSimulator.Controllers.Dtos.Responses;

namespace MonteCarloSimulator.Result;

public interface IGetResultRepository
{
    public IEnumerable<GetResultResponseDto> GetResults(IEnumerable<string> simulationIds);
}
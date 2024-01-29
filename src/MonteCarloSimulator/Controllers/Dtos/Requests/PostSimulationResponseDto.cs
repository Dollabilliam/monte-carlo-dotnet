using System.Text.Json.Serialization;

namespace MonteCarloSimulator.Controllers.Dtos.Requests;

public record PostSimulationRequestDto
{
    [JsonPropertyName("timeSteps")]
    public int TimeSteps { get; init; }

    [JsonPropertyName("scenarios")]
    public int Scenarios { get; init; }

    [JsonPropertyName("assets")]
    public Asset[] Assets { get; init; }
}
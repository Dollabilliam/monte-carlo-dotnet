using System.Text.Json.Serialization;

namespace MonteCarloSimulator.Controllers.Dtos.Requests;

public record PostSimulationRequestDto
{
    [JsonPropertyName("T")]
    public int TimeSteps { get; init; }

    [JsonPropertyName("S")]
    public int Scenarios { get; init; }

    [JsonPropertyName("assets")]
    public Asset[] Assets { get; init; }
}
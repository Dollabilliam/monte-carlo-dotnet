using System.Text.Json.Serialization;

namespace MonteCarloSimulator.Controllers.Dtos;

public record Asset
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("p0")]
    public double InitialPrice { get; init; }

    [JsonPropertyName("m")]
    public string Mean { get; init; }

    [JsonPropertyName("s")]
    public string StandardDeviation { get; init; }
}
using System.Text.Json.Serialization;

namespace MonteCarloSimulator.Controllers.Dtos;

public record Asset
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("initialPrice")]
    public double InitialPrice { get; init; }

    [JsonPropertyName("mean")]
    public string Mean { get; init; }

    [JsonPropertyName("standardDeviation")]
    public string StandardDeviation { get; init; }
}
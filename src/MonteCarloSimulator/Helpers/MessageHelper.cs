using MonteCarloSimulator.Controllers.Dtos;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.ScenarioProcessor;

namespace MonteCarloSimulator.Helpers;

public static class MessageHelper
{
    public static QueueMessage CreateMessage(string simulationId, int scenarios, int timeSteps, Asset asset)
    {
        var meanAsDouble = ConvertPercentageToDouble(asset.Mean);
        var stdDevAsDouble = ConvertPercentageToDouble(asset.StandardDeviation);

        if (meanAsDouble == 0 | stdDevAsDouble == 0)
        {
            throw new Exception("Invalid mean and standard deviation was provided. Valid examples include '0.01%' and '5%'");
        }

        return new QueueMessage
        {
            SimulationObject = new SimulationObject
            {
                SimulationId = simulationId,
                Scenarios = scenarios,
                TimeSteps = timeSteps,
                InitialPrice = asset.InitialPrice,
                Mean = meanAsDouble,
                StandardDeviation = stdDevAsDouble
            }
        };
    }

    private static double ConvertPercentageToDouble(string percentage)
    {
        if (double.TryParse(percentage.TrimEnd('%'), out double result))
        {
            return result / 100;
        }

        return 0;
    }
}
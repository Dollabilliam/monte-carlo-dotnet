using MonteCarloSimulator.Queues;
using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator;

public class QueueWorker : BackgroundService
{

    private readonly ILogger<QueueWorker> logger;
    private readonly IDequeueQueue<QueueMessage> queue;

    public QueueWorker(ILogger<QueueWorker> logger, IDequeueQueue<QueueMessage> queue)
    {
        this.logger = logger;
        this.queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await Task.Delay(10000, cancellationToken);

            var message = await queue.Dequeue();
            if (message != null)
            {
                logger.LogInformation("Message {messageName} will be processed", message.SimulationObject.SimulationId);

                // await processor.ProcessScenarios(message, cancellationToken);

                logger.LogInformation("Message {messageName} has been processed", message.SimulationObject.SimulationId);
            }

            logger.LogInformation("No message in queue");
        }
    }
}
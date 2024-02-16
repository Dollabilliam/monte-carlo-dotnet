using Microsoft.Extensions.Options;
using MonteCarloSimulator.Options;
using MonteCarloSimulator.Queues;
using MonteCarloSimulator.Queues.Messages;
using MonteCarloSimulator.ScenarioProcessor;

namespace MonteCarloSimulator;

public class QueueWorker : BackgroundService
{
    private readonly ILogger<QueueWorker> logger;
    private readonly IDequeueQueue<QueueMessage> queue;
    private readonly IScenarioProcessor processor;
    private readonly IOptions<QueueWorkerOptions> options;

    public QueueWorker(
        ILogger<QueueWorker> logger,
        IDequeueQueue<QueueMessage> queue,
        IScenarioProcessor processor,
        IOptions<QueueWorkerOptions> options)
    {
        this.logger = logger;
        this.queue = queue;
        this.processor = processor;
        this.options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        { 
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await Task.Delay(options.Value.WorkerDelayInMs ?? 10000, cancellationToken);

            var message = queue.Dequeue();
            if (message != null)
            {
                logger.LogInformation("Message {messageName} will be processed", message.SimulationObject.SimulationId);

                await processor.ProcessScenarios(message, cancellationToken);

                logger.LogInformation("Message {messageName} has been processed", message.SimulationObject.SimulationId);
            }

            logger.LogInformation("No message in queue");
        }
    }
}
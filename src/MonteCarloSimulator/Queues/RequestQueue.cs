using System.Collections.Concurrent;
using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator.Queues;

public class RequestQueue : IEnqueueQueue<QueueMessage>, IDequeueQueue<QueueMessage>
{
    private static readonly ConcurrentQueue<QueueMessage> Messages = new();
    private readonly ILogger<RequestQueue> logger;

    public RequestQueue(ILogger<RequestQueue> logger)
    {
        this.logger = logger;
    }

    public void Enqueue(QueueMessage message)
    {
        if (message == null)
        {
            logger.LogWarning("message-to-enque was null");
            return;
        }

        Messages.Enqueue(message);
    }

    public QueueMessage Dequeue()
    {
        var success = Messages.TryDequeue(out var message);

        return success ? message : null;
    }
}
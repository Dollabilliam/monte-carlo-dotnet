using System.Collections.Concurrent;
using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator.Queues;

public class RequestQueue : IEnqueueQueue<QueueMessage>, IDequeueQueue<QueueMessage>
{
    private static readonly ConcurrentQueue<QueueMessage> Messages = new();

    public void Enqueue(QueueMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        Messages.Enqueue(message);
    }

    public QueueMessage Dequeue()
    {
        var success = Messages.TryDequeue(out var message);

        return success ? message : null;
    }
}
using System.Collections.Concurrent;
using MonteCarloSimulator.Queues.Messages;

namespace MonteCarloSimulator.Queues;

public class RequestQueue : IEnqueueRequest<QueueMessage>
{
    private static readonly ConcurrentQueue<QueueMessage> Messages = new();

    public Task Enqueue(QueueMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        Messages.Enqueue(message);
        return Task.CompletedTask;
    }

    public Task<QueueMessage?> Dequeue()
    {
        var success = Messages.TryDequeue(out var message);

        return Task.FromResult(success ? message : null);
    }
}
namespace MonteCarloSimulator.Queues;

public interface IEnqueueRequest<T>
{
    Task Enqueue(T message);
}
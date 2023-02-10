namespace MonteCarloSimulator.Queues;

public interface IDequeueRequest<T>
{
    Task<T> Dequeue();
}
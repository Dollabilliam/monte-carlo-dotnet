namespace MonteCarloSimulator.Queues;

public interface IDequeueQueue<T>
{
    Task<T> Dequeue();
}
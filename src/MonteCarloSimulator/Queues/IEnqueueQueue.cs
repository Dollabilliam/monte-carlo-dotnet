namespace MonteCarloSimulator.Queues;

public interface IEnqueueQueue<T>
{
    Task Enqueue(T message);
}
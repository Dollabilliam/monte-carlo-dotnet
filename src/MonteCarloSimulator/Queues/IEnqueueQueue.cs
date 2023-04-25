namespace MonteCarloSimulator.Queues;

public interface IEnqueueQueue<T>
{
    void Enqueue(T message);
}
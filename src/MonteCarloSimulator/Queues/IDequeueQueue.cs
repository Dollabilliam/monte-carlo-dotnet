namespace MonteCarloSimulator.Queues;

public interface IDequeueQueue<T>
{
    T Dequeue();
}
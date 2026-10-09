namespace MathForge.Noises.Abstractions;

public interface INoise1D<T>
{
	public T Sample(T x);
}
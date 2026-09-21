namespace MathForge.Graphics.Noises.Abstractions;

public interface INoise1D<T>
{
	public T Sample(T x);
}
namespace MathForge.Noises.Abstractions;

public interface INoise4D
{
	public float Sample(float x, float y, float z, float w);
}
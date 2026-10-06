namespace MathForge.Graphs.Entities;

public sealed class GraphPath<T>
{
	public IReadOnlyList<T> Vertices { get; }

	public GraphPath(IReadOnlyList<T> vertices)
	{
		Vertices = vertices;
	}
}
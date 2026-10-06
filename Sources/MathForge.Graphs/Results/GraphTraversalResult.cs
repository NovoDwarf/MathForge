namespace MathForge.Graphs.Results;

public sealed class GraphTraversalResult<T>
{
	public IReadOnlyList<T> Order { get; }

	public IReadOnlyDictionary<T, T?> Parents { get; }

	public GraphTraversalResult(IReadOnlyList<T> order, IReadOnlyDictionary<T, T?> parents)
	{
		Order = order;
		Parents = parents;
	}
}
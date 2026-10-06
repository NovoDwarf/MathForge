namespace MathForge.Graphs.Results;

public sealed class TopologicalSortResult<TVertex>
{
	public bool IsAcyclic { get; }

	public IReadOnlyList<TVertex> Order { get; }

	public TopologicalSortResult(bool isAcyclic, IReadOnlyList<TVertex> order)
	{
		IsAcyclic = isAcyclic;
		Order = order;
	}
}
namespace MathForge.Graphs.Results;

public sealed class ShortestPathResult<TVertex, TDistance>
{
	public bool Found { get; }

	public TDistance Distance { get; }

	public IReadOnlyList<TVertex> Path { get; }

	public ShortestPathResult(bool found, TDistance distance, IReadOnlyList<TVertex> path)
	{
		Found = found;
		Distance = distance;
		Path = path;
	}
}
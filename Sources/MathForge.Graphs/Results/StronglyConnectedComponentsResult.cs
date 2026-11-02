namespace MathForge.Graphs.Results;

public sealed class StronglyConnectedComponentsResult<T>
{
	public IReadOnlyList<IReadOnlySet<T>> Components { get; }

	public int Count => Components.Count;

	public StronglyConnectedComponentsResult(IReadOnlyList<IReadOnlySet<T>> components)
	{
		Components = components;
	}
}
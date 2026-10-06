namespace MathForge.Graphs.Results;

public sealed class ConnectedComponentsResult<T>
{
	public IReadOnlyList<IReadOnlySet<T>> Components { get; }

	public int Count => Components.Count;

	public ConnectedComponentsResult(IReadOnlyList<IReadOnlySet<T>> components)
	{
		Components = components;
	}
}
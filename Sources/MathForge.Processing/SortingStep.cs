namespace MathForge.Processing;

public sealed class SortingStep<T>
{
	public required T[] Array { get; init; }

	public int Left { get; init; }
	public int Right { get; init; }
	public int? Middle { get; init; }
}
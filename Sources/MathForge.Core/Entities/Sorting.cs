namespace MathForge.Core.Entities;

public abstract class Sorting<T> : Entity
	where T : IComparable<T>
{
	public Action<SortingStep<T>>? OnStep { get; set; }

	public abstract void Sort(T[] array);
}

public sealed class SortingStep<T>
{
	public required T[] Array { get; init; }

	public int Left { get; init; }
	public int Right { get; init; }
	public int? Middle { get; init; }
}
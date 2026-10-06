using MathForge.Core;

namespace MathForge.Processing;

public abstract class Sorting<T> : MathEntity
	where T : IComparable<T>
{
	public Action<SortingStep<T>>? OnStep { get; set; }

	public abstract void Sort(T[] array);
}
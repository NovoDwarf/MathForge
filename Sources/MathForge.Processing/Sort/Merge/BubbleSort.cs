using MathForge.Core.Entities;

namespace MathForge.Processing.Sort.Merge;

public sealed class BubbleSort<T> : Sorting<T>
	where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		ArgumentNullException.ThrowIfNull(array);

		for (var i = array.Length - 1; i > 0; i--)
		{
			var swapped = false;

			for (var j = 0; j < i; j++)
			{
				if (array[j].CompareTo(array[j + 1]) <= 0)
					continue;

				(array[j], array[j + 1]) = (array[j + 1], array[j]);

				swapped = true;

				OnStep?.Invoke(new SortingStep<T> { Array = (T[])array.Clone(), Left = j, Right = j + 1 });
			}

			if (!swapped)
				break;
		}
	}
}
using MathForge.Core.Attributes;

namespace MathForge.Processing.Sort.Exchange;

[Categories("Sort", "Exchange")]
public sealed class CocktailShakerSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		var start = 0;
		var end = array.Length - 1;

		while (start < end)
		{
			var swapped = false;

			for (var i = start; i < end; i++)
			{
				if (array[i].CompareTo(array[i + 1]) <= 0)
					continue;

				(array[i], array[i + 1]) = (array[i + 1], array[i]);

				swapped = true;

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = i,
					Right = i + 1
				});
			}

			if (!swapped)
				break;

			end--;
			swapped = false;

			for (var i = end; i > start; i--)
			{
				if (array[i - 1].CompareTo(array[i]) <= 0)
					continue;

				(array[i - 1], array[i]) = (array[i], array[i - 1]);

				swapped = true;

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = i - 1,
					Right = i
				});
			}

			if (!swapped)
				break;

			start++;
		}
	}
}
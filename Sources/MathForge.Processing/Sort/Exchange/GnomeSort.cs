using MathForge.Core.Attributes;

namespace MathForge.Processing.Sort.Exchange;

[Categories("Sort", "Exchange")]
public sealed class GnomeSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		var i = 1;

		while (i < array.Length)
		{
			if (i == 0 || array[i - 1].CompareTo(array[i]) <= 0)
			{
				i++;
				continue;
			}

			(array[i - 1], array[i]) = (array[i], array[i - 1]);

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = i - 1,
				Right = i
			});

			i--;
		}
	}
}
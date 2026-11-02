namespace MathForge.Processing.Sort.Insertion;

public sealed class BinaryInsertionSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		for (var i = 1; i < array.Length; i++)
		{
			var value = array[i];
			var left = 0;
			var right = i;

			while (left < right)
			{
				var middle = left + (right - left) / 2;

				if (array[middle].CompareTo(value) <= 0)
					left = middle + 1;
				else
					right = middle;
			}

			for (var j = i; j > left; j--)
			{
				array[j] = array[j - 1];

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = j - 1,
					Right = j,
					Middle = left
				});
			}

			if (left == i)
				continue;

			array[left] = value;

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = left,
				Right = i,
				Middle = left
			});
		}
	}
}
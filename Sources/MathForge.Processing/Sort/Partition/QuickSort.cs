namespace MathForge.Processing.Sort.Partition;

public sealed class QuickSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		Quick(array, 0, array.Length - 1);
	}

	private void Quick(T[] array, int left, int right)
	{
		if (left >= right)
			return;

		var pivot = array[left + (right - left) / 2];
		var i = left;
		var j = right;

		while (i <= j)
		{
			while (array[i].CompareTo(pivot) < 0)
				i++;

			while (array[j].CompareTo(pivot) > 0)
				j--;

			if (i > j)
				continue;

			if (i != j)
			{
				(array[i], array[j]) = (array[j], array[i]);

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = i,
					Right = j,
					Middle = left + (right - left) / 2
				});
			}

			i++;
			j--;
		}

		if (left < j)
			Quick(array, left, j);

		if (i < right)
			Quick(array, i, right);
	}
}
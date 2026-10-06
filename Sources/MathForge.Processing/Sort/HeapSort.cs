namespace MathForge.Processing.Sort.Randomized;

public sealed class HeapSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		var length = array.Length;

		for (var i = length / 2 - 1; i >= 0; i--)
			Heapify(array, length, i);

		for (var i = length - 1; i > 0; i--)
		{
			(array[0], array[i]) = (array[i], array[0]);

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = 0,
				Right = i
			});

			Heapify(array, i, 0);
		}
	}

	private void Heapify(T[] array, int length, int root)
	{
		while (true)
		{
			var largest = root;
			var left = 2 * root + 1;
			var right = 2 * root + 2;

			if (left < length && array[left].CompareTo(array[largest]) > 0) 
				largest = left;

			if (right < length && array[right].CompareTo(array[largest]) > 0) 
				largest = right;

			if (largest == root)
				return;

			(array[root], array[largest]) = (array[largest], array[root]);

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = root,
				Right = largest,
				Middle = left < length ? left : null
			});

			root = largest;
		}
	}
}
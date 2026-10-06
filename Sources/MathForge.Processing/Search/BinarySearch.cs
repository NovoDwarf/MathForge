namespace MathForge.Processing.Search;

public sealed class BinarySearch<T> where T : IComparable<T>
{
	public Action<SearchStep<T>>? OnStep { get; set; }

	public int Search(ReadOnlySpan<T> array, T value)
	{
		var left = 0;
		var right = array.Length - 1;

		while (left <= right)
		{
			var middle = left + (right - left) / 2;

			OnStep?.Invoke(new SearchStep<T>
			{
				Left = left,
				Middle = middle,
				Right = right,
				Value = value,
				Current = array[middle]
			});

			var comparison = array[middle].CompareTo(value);

			if (comparison == 0)
				return middle;

			if (comparison < 0)
				left = middle + 1;
			else
				right = middle - 1;
		}

		return -1;
	}
}
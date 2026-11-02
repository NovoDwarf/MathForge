namespace MathForge.Processing.Sort.Merge;

public sealed class TimSort<T> : Sorting<T> where T : IComparable<T>
{
	private const int MinRun = 32;

	public override void Sort(T[] array)
	{
		var length = array.Length;

		for (var start = 0; start < length; start += MinRun)
		{
			InsertionSort(array, start, Math.Min(start + MinRun - 1, length - 1));
		}

		for (var size = MinRun; size < length; size *= 2)
		{
			for (var left = 0; left < length; left += size * 2)
			{
				var middle = left + size - 1;
				var right = Math.Min(left + size * 2 - 1, length - 1);

				if (middle >= right)
					continue;

				Merge(array, left, middle, right);
			}

			if (size > length / 2)
				break;
		}
	}

	private void InsertionSort(T[] array, int left, int right)
	{
		for (var i = left + 1; i <= right; i++)
		{
			var value = array[i];
			var j = i - 1;

			while (j >= left && array[j].CompareTo(value) > 0)
			{
				array[j + 1] = array[j];

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = j,
					Right = j + 1
				});

				j--;
			}

			array[j + 1] = value;

			if (j + 1 != i)
			{
				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = j + 1,
					Right = i
				});
			}
		}
	}

	private void Merge(T[] array, int left, int middle, int right)
	{
		var leftLength = middle - left + 1;
		var rightLength = right - middle;

		var leftArray = new T[leftLength];
		var rightArray = new T[rightLength];

		Array.Copy(array, left, leftArray, 0, leftLength);
		Array.Copy(array, middle + 1, rightArray, 0, rightLength);

		var i = 0;
		var j = 0;
		var k = left;

		while (i < leftLength && j < rightLength)
		{
			if (leftArray[i].CompareTo(rightArray[j]) <= 0)
				array[k++] = leftArray[i++];
			else
				array[k++] = rightArray[j++];

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = left,
				Right = right,
				Middle = middle
			});
		}

		while (i < leftLength)
		{
			array[k++] = leftArray[i++];

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = left,
				Right = right,
				Middle = middle
			});
		}

		while (j < rightLength)
		{
			array[k++] = rightArray[j++];

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = left,
				Right = right,
				Middle = middle
			});
		}
	}
}
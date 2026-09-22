using MathForge.Core.Entities;

namespace MathForge.Processing.Sort.Merge;

public sealed class MergeSort<T> : Sorting<T>
	where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		ArgumentNullException.ThrowIfNull(array);

		if (array.Length < 2)
			return;

		var buffer = new T[array.Length];

		Sort(array, buffer, 0, array.Length - 1);
	}

	private void Sort(T[] array, T[] buffer, int left, int right)
	{
		if (left >= right)
			return;

		var middle = left + (right - left) / 2;

		OnStep?.Invoke(new SortingStep<T>
		{
			Array = (T[])array.Clone(),
			Left = left,
			Right = right,
			Middle = middle
		});

		Sort(array, buffer, left, middle);
		Sort(array, buffer, middle + 1, right);
		Merge(array, buffer, left, middle, right);
	}

	private void Merge(
		T[] array,
		T[] buffer,
		int left,
		int middle,
		int right)
	{
		var leftIndex = left;
		var rightIndex = middle + 1;
		var bufferIndex = left;

		while (leftIndex <= middle && rightIndex <= right)
		{
			if (array[leftIndex].CompareTo(array[rightIndex]) <= 0)
			{
				buffer[bufferIndex++] = array[leftIndex++];
			}
			else
			{
				buffer[bufferIndex++] = array[rightIndex++];
			}
		}

		while (leftIndex <= middle)
			buffer[bufferIndex++] = array[leftIndex++];

		while (rightIndex <= right)
			buffer[bufferIndex++] = array[rightIndex++];

		for (var i = left; i <= right; i++)
			array[i] = buffer[i];

		OnStep?.Invoke(new SortingStep<T> { Array = (T[])array.Clone(), Left = left, Right = right, Middle = middle });
	}
}
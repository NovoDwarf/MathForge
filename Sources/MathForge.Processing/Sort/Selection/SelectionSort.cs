namespace MathForge.Processing.Sort.Selection;

public sealed class SelectionSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		for (var i = 0; i < array.Length - 1; i++)
		{
			var minIndex = i;

			for (var j = i + 1; j < array.Length; j++)
			{
				if (array[j].CompareTo(array[minIndex]) < 0)
					minIndex = j;
			}

			if (minIndex == i)
				continue;

			(array[i], array[minIndex]) = (array[minIndex], array[i]);

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = i,
				Right = minIndex
			});
		}
	}
}
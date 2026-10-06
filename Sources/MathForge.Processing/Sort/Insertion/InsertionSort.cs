namespace MathForge.Processing.Sort.Insertion;

public sealed class InsertionSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		for (var i = 1; i < array.Length; i++)
		{
			var j = i;

			while (j > 0 && array[j - 1].CompareTo(array[j]) > 0)
			{
				(array[j - 1], array[j]) = (array[j], array[j - 1]);

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = (T[])array.Clone(),
					Left = j - 1,
					Right = j
				});

				j--;
			}
		}
	}
}
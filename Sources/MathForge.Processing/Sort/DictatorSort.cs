namespace MathForge.Processing.Sort.Randomized;

public sealed class DictatorSort<T> : Sorting<T> where T : IComparable<T>
{
	public override void Sort(T[] array)
	{
		var items = new List<T>(array);

		for (var i = 0; i < items.Count - 1;)
		{
			if (items[i].CompareTo(items[i + 1]) > 0)
			{
				items.RemoveAt(i);

				OnStep?.Invoke(new SortingStep<T>
				{
					Array = CreateSnapshot(items, array.Length),
					Left = i,
					Right = i + 1
				});

				if (i > 0)
					i--;

				continue;
			}

			i++;
		}

		Array.Clear(array);

		for (var i = 0; i < items.Count; i++)
			array[i] = items[i];
	}

	private static T[] CreateSnapshot(List<T> items, int length)
	{
		var snapshot = new T[length];

		for (var i = 0; i < items.Count; i++)
			snapshot[i] = items[i];

		return snapshot;
	}
}
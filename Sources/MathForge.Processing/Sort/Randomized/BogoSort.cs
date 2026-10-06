using MathForge.Random.Generators;

namespace MathForge.Processing.Sort.Randomized;

public sealed class BogoSort<T> : Sorting<T> where T : IComparable<T>
{
	private readonly IRandom _random;

	public BogoSort(IRandom random)
	{
		_random = random;
	}
	
	public override void Sort(T[] array)
	{
		while (!IsSorted(array))
		{
			Shuffle(array);

			OnStep?.Invoke(new SortingStep<T>
			{
				Array = (T[])array.Clone(),
				Left = 0,
				Right = array.Length - 1
			});
		}
	}

	private static bool IsSorted(T[] array)
	{
		for (var i = 1; i < array.Length; i++)
		{
			if (array[i - 1].CompareTo(array[i]) > 0)
				return false;
		}

		return true;
	}

	private void Shuffle(T[] array)
	{
		for (var i = array.Length - 1; i > 0; i--)
		{
			var j = _random.Next(i + 1);

			(array[i], array[j]) = (array[j], array[i]);
		}
	}
}
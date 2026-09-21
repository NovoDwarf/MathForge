using MathForge.Random.Generators;

namespace MathForge.Random;

public sealed class PermutationTable
{
	private readonly int[] _table;

	public int Size { get; }

	public PermutationTable(IRandom generator, int size = 256, long seed = 0)
	{
		if (size <= 0 || (size & (size - 1)) != 0)
			throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive power of two.");

		Size = size;

		var permutation = new int[size];

		for (var i = 0; i < size; i++)
			permutation[i] = i;

		for (var i = size - 1; i > 0; i--)
		{
			var j = generator.Next(i + 1);
			(permutation[i], permutation[j]) = (permutation[j], permutation[i]);
		}

		_table = new int[size * 2];

		for (var i = 0; i < _table.Length; i++)
			_table[i] = permutation[i & (size - 1)];
	}

	public int this[int index] => _table[index & (_table.Length - 1)];

	public int Hash(int x) => this[x];
	public int Hash(int x, int y) => this[this[x] + y];
	public int Hash(int x, int y, int z) => this[this[this[x] + y] + z];
	public int Hash(int x, int y, int z, int w) => this[this[this[this[x] + y] + z] + w];
}
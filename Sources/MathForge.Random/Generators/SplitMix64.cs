namespace MathForge.Random.Generators;

public sealed class SplitMix64 : IRandom
{
	private ulong _state;

	public SplitMix64(long seed = 0)
	{
		_state = unchecked((ulong)seed);
	}

	public ulong NextUInt64()
	{
		_state += 0x9E3779B97F4A7C15UL;

		var z = _state;
		z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
		z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;

		return z ^ (z >> 31);
	}

	public uint NextUInt32() => (uint)NextUInt64();

	public int Next() => Next(int.MaxValue);

	public int Next(int max)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(max);

		var range = (uint)max;
		var limit = uint.MaxValue - (uint.MaxValue % range);

		uint value;

		do
		{
			value = NextUInt32();
		}
		while (value >= limit);

		return (int)(value % range);
	}

	public int Next(int min, int max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

		var range = (uint)((long)max - min);

		return min + (int)(NextUInt64() % range);
	}

	public void NextBytes(Span<byte> buffer)
	{
		var offset = 0;

		while (offset + sizeof(ulong) <= buffer.Length)
		{
			var value = NextUInt64();

			BitConverter.TryWriteBytes(
				buffer[offset..],
				value);

			offset += sizeof(ulong);
		}

		if (offset < buffer.Length)
		{
			var value = NextUInt64();

			for (var i = offset; i < buffer.Length; i++)
			{
				buffer[i] = (byte)value;
				value >>= 8;
			}
		}
	}
	
	public double NextDouble()
		=> (NextUInt64() >> 11) * (1.0 / (1UL << 53));

	public double NextDouble(double min, double max)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

		return min + NextDouble() * (max - min);
	}
	
	public float NextSingle()
		=> (float)NextDouble();

	public bool NextBoolean()
		=> (NextUInt64() & 1) != 0;
}
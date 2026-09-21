using MathForge.Graphics.Noises.Options;
using MathForge.Random;

namespace MathForge.Graphics.Noises.Gradient;

public partial class PerlinNoise
{
	private readonly PerlinNoiseOptions _options;
	private readonly PermutationTable _permutation;

	public PerlinNoise(PerlinNoiseOptions? options = null)
	{
		if (options != null && (options.Size <= 0 || (options.Size & (options.Size - 1)) != 0))
			throw new ArgumentOutOfRangeException(nameof(options.Size), "Size must be a power of two.");

		_options = options ?? new PerlinNoiseOptions();
		_permutation = new PermutationTable(_options.Random, _options.Size, _options.Seed == -1 ? _options.Random.Next() : _options.Seed);
	}

	private void ApplyInput(ref float x, ref float y, ref float z)
	{
		x = x * _options.Scale + _options.Offset.X;
		y = y * _options.Scale + _options.Offset.Y;
		z = z * _options.Scale + _options.Offset.Z;
	}

	private float ApplyOutput(float value)
	{
		if (_options.Invert)
			value = 1f - value;

		if (_options.Power != 1f)
			value = MathF.Pow(MathF.Abs(value), _options.Power);

		value = value * _options.Amplitude + _options.Bias;

		if (!_options.Clamp01)
			return value;

		value = value switch
		{
			< 0 => 0,
			> 1 => 1,
			_ => value
		};

		return value;
	}

	private static float Fade(float t)
	{
		return t * t * t * (t * (t * 6 - 15) + 10);
	}

	private static float Lerp(float t, float a, float b)
	{
		return a + t * (b - a);
	}
}
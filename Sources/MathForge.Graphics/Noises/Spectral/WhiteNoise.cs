using System.Runtime.CompilerServices;
using MathForge.Graphics.Noises.Abstractions;
using MathForge.Graphics.Noises.Options;

namespace MathForge.Graphics.Noises.Spectral;

public sealed partial class WhiteNoise : INoise1D<float>, INoise2D, INoise3D
{
	private readonly WhiteNoiseOptions _options;

	public WhiteNoise(WhiteNoiseOptions? options = null)
	{
		_options = options ?? new WhiteNoiseOptions();

		//_options.Seed = _options.Seed == -1 ? _options.Random.Next() : _options.Seed;
	}

	private float ApplyOutput(float value)
	{
		if (_options.Invert)
			value = -value;

		value = value * _options.Amplitude + _options.Bias;

		if (_options.Normalize)
			value = value * 0.5f + 0.5f;

		if (_options.Power != 1f)
			value = MathF.Pow(value, _options.Power);

		if (_options.Clamp01)
			value = Math.Clamp(value, 0f, 1f);

		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint Hash(float x, float y, float z, int seed)
	{
		unchecked
		{
			var h = (uint)BitConverter.SingleToInt32Bits(x);

			h ^= (uint)BitConverter.SingleToInt32Bits(y) * 0x9E3779B9u;
			h ^= (uint)BitConverter.SingleToInt32Bits(z) * 0x85EBCA6Bu;
			h ^= (uint)seed * 0xC2B2AE35u;

			h ^= h >> 16;
			h *= 0x85EBCA6Bu;
			h ^= h >> 13;
			h *= 0xC2B2AE35u;
			h ^= h >> 16;

			return h;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float ToNoise(uint hash)
	{
		return (hash & 0xFFFFFF) / 16777215f * 2f - 1f;
	}
}

public record WhiteNoiseOptions : SeedNoiseOptions
{
}
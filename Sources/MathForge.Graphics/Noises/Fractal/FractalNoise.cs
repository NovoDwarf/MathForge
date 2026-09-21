using MathForge.Graphics.Noises.Abstractions;
using MathForge.Graphics.Noises.Options;

namespace MathForge.Graphics.Noises.Fractal;

public class FractalNoise : INoise1D<float>, INoise2D, INoise3D
{
	public FractalNoise(INoise3D noise, FractalNoiseOptions? options = null)
	{
		Options = options ?? new FractalNoiseOptions();
		Noise = noise;
	}

	private INoise3D Noise { get; }
	private FractalNoiseOptions Options { get; }

	public float Sample(float x)
	{
		return Fractal(x, 0, 0);
	}

	public float Sample(float x, float y)
	{
		return Fractal(x, y, 0);
	}

	public float Sample(float x, float y, float z)
	{
		return Fractal(x, y, z);
	}

	private float Fractal(float x, float y, float z)
	{
		x = x * Options.Scale + Options.Offset.X;
		y = y * Options.Scale + Options.Offset.Y;
		z = z * Options.Scale + Options.Offset.Z;

		var total = 0f;
		var frequency = 1f;
		var amplitude = 1f;
		var maxValue = 0f;

		for (var i = 0; i < Options.Octaves; i++)
		{
			var n = Noise.Sample(x * frequency, y * frequency, z * frequency);

			switch (Options.Type)
			{
				case FractalType.Billow:
					n = Math.Abs(n);
					break;

				case FractalType.Turbulence:
					n = 2f * MathF.Abs(n) - 1f;
					break;

				case FractalType.Ridged:
					n = 1f - Math.Abs(n);
					n *= n;
					break;

				case FractalType.FBM:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			total += n * amplitude;
			maxValue += amplitude;

			amplitude *= Options.Persistence;
			frequency *= Options.Lacunarity;
		}

		var value = Options.Normalize ? total / maxValue : total;

		if (Options.Power != 1f)
			value = MathF.Pow(value, Options.Power);

		if (Options.Invert)
			value = 1f - value;

		value = value * Options.Amplitude + Options.Bias;

		if (!Options.Clamp01)
			return value;

		value = value switch
		{
			< 0 => 0,
			> 1 => 1,
			_ => value
		};

		return value;
	}
}
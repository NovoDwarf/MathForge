using System.Numerics;
using MathForge.Random.Generators;

namespace MathForge.Graphics.Noises.Options;

public abstract record BaseNoiseOptions
{
	public IRandom Random { get; init; } = new SplitMix64();
	
	public float Scale { get; init; } = 1.0f;
	public Vector4 Offset { get; init; } = new(0, 0, 0, 0);

	public float Amplitude { get; init; } = 1.0f;
	public float Bias { get; init; } = 0.0f;

	public bool Clamp01 { get; init; } = true;
	public bool Normalize { get; init; } = true;
	public float Power { get; init; } = 1.0f;
	public bool Invert { get; init; } = false;
	
}

public abstract record SeedNoiseOptions : BaseNoiseOptions
{
	public long Seed { get; init; } = -1;
}
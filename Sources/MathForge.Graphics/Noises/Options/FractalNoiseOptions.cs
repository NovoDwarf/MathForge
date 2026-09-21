namespace MathForge.Graphics.Noises.Options;

public record FractalNoiseOptions : BaseNoiseOptions
{
	public int Octaves { get; init; } = 6;
	public float Persistence { get; init; } = 0.5f;
	public float Lacunarity { get; init; } = 2.0f;
	public FractalType Type { get; init; } = FractalType.FBM;
}
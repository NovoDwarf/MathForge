namespace MathForge.Graphics.Noises.Options;

public record OpenSimplexOptions : SeedNoiseOptions
{
	public OpenSimplexNoise2DType Type2D { get; init; } = OpenSimplexNoise2DType.Default;
	public OpenSimplexNoise3DType Type3D { get; init; } = OpenSimplexNoise3DType.Unrotated;
	public OpenSimplexNoise4DType Type4D { get; init; } = OpenSimplexNoise4DType.Unskewed;
}
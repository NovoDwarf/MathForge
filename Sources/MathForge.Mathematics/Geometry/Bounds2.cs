using MathForge.Vectors.Float;

namespace MathForge.Geometry;

public readonly record struct Bounds2(Float2 Min, Float2 Max)
{
	public float Width => Max.X - Min.X;
	public float Height => Max.Y - Min.Y;
}
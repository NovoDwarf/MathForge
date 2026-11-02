using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Geometry.Primitives;

public readonly record struct Point2(float X, float Y)
{
	public Float2 Position => new(X, Y);
}
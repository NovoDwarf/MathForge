using MathForge.Attributes;
using MathForge.Utilities;

namespace MathForge.LinearAlgebra.Vectors;

[Vector(2)]
public readonly partial record struct Float2
{
	public Float2(float x, float y)
	{
		X = x;
		Y = y;
	}
	
	public Float2(double x, double y)
	{
		X = (float)x;
		Y = (float)y;
	}

	public float X { get; }
	public float Y { get; }
	
	public static bool IsInside(Float2 point, Float2 edgePoint, Float2 edgeNormal)
	{
		return VectorUtils.IsInside(point.X, point.Y, edgePoint.X, edgePoint.Y, edgeNormal.X, edgeNormal.Y);
	}

	public static Float2 ComputeIntersection(Float2 a, Float2 b, Float2 edgePoint, Float2 edgeNormal)
	{
		var coords = VectorUtils.ComputeIntersection(a.X, a.Y, b.X, b.Y, edgePoint.X, edgePoint.Y, edgeNormal.X, edgeNormal.Y);
		
		return new Float2(coords.X, coords.Y);
	}
}
using MathForge.Utilities;

namespace MathForge.Vectors.Float;

public readonly partial record struct Float2
{
	public static float Dot(Float2 a, Float2 b)
	{
		return VectorUtils.Dot(a.X, a.Y, b.X, b.Y);
	}

	public static float Cross(Float2 a, Float2 b)
	{
		return VectorUtils.Cross(a.X, a.Y, b.X, b.Y);
	}

	public static float DistanceSquared(Float2 a, Float2 b)
	{
		return (a - b).LengthSquared;
	}

	public static float Distance(Float2 a, Float2 b)
	{
		return (a - b).Length;
	}

	public static Float2 Lerp(Float2 a, Float2 b, float t)
	{
		return a + (b - a) * t;
	}

	public static bool IsInside(Float2 point, Float2 edgePoint, Float2 edgeNormal)
	{
		return VectorUtils.IsInside(point.X, point.Y, edgePoint.X, edgePoint.Y, edgeNormal.X, edgeNormal.Y);
	}

	public static Float2 ComputeIntersection(Float2 a, Float2 b, Float2 edgePoint, Float2 edgeNormal)
	{
		var coords = VectorUtils.ComputeIntersection(a.X, a.Y, b.X, b.Y, edgePoint.X, edgePoint.Y, edgeNormal.X, edgeNormal.Y);
		
		return new Float2(coords.X, coords.Y);
	}
	
	public static Float2 Min(Float2 a, Float2 b)
	{
		return new Float2(VectorUtils.Min(a.X, b.X), VectorUtils.Min(a.Y, b.Y));
	}

	public static Float2 Max(Float2 a, Float2 b)
	{
		return new Float2(VectorUtils.Max(a.X, b.X), VectorUtils.Max(a.Y, b.Y));
	}
}
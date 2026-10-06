using MathForge.Utilities;

namespace MathForge.Vectors.Float;

public readonly partial record struct Float3
{
	public static float Dot(Float3 a, Float3 b)
	{
		return VectorUtils.Dot(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
	}

	public static Float3 Cross(Float3 a, Float3 b)
	{
		return new Float3(VectorUtils.CrossX(a.X, a.Y, a.Z, b.X, b.Y, b.Z),
			VectorUtils.CrossY(a.X, a.Y, a.Z, b.X, b.Y, b.Z),
			VectorUtils.CrossZ(a.X, a.Y, a.Z, b.X, b.Y, b.Z));
	}

	public static float DistanceSquared(Float3 a, Float3 b)
	{
		return (a - b).LengthSquared;
	}

	public static float Distance(Float3 a, Float3 b)
	{
		return (a - b).Length;
	}

	public static Float3 Lerp(Float3 a, Float3 b, float t)
	{
		return a + (b - a) * t;
	}

	public static Float3 Min(Float3 a, Float3 b)
	{
		return new Float3(VectorUtils.Min(a.X, b.X), VectorUtils.Min(a.Y, b.Y), VectorUtils.Min(a.Z, b.Z));
	}

	public static Float3 Max(Float3 a, Float3 b)
	{
		return new Float3(VectorUtils.Max(a.X, b.X), VectorUtils.Max(a.Y, b.Y), VectorUtils.Max(a.Z, b.Z));
	}
}
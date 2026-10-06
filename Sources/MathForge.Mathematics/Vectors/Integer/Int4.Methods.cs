using MathForge.Utilities;

namespace MathForge.Vectors.Integer;

public readonly partial record struct Int4
{
	public static int Dot(Int4 a, Int4 b)
	{
		return VectorUtils.Dot(
			a.X, a.Y, a.Z, a.W, 
			b.X, b.Y, b.Z, b.W);
	}
	
	public static int DistanceSquared(Int4 a, Int4 b)
	{
		return (a - b).LengthSquared;
	}

	public static double Distance(Int4 a, Int4 b)
	{
		return (a - b).Length;
	}

	public static Int4 Min(Int4 a, Int4 b)
	{
		return new Int4(VectorUtils.Min(a.X, b.X), VectorUtils.Min(a.Y, b.Y), VectorUtils.Min(a.Z, b.Z), VectorUtils.Min(a.W, b.W));
	}

	public static Int4 Max(Int4 a, Int4 b)
	{
		return new Int4(VectorUtils.Max(a.X, b.X), VectorUtils.Max(a.Y, b.Y), VectorUtils.Max(a.Z, b.Z), VectorUtils.Min(a.W, b.W));
	}
}
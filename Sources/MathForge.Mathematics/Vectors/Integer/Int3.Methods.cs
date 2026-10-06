using MathForge.Utilities;

namespace MathForge.Vectors.Integer;

public readonly partial record struct Int3
{
	public static int Dot(Int3 a, Int3 b)
	{
		return VectorUtils.Dot(a.X, a.Y, a.Z, b.X, b.Y, b.Z);
	}

	public static Int3 Cross(Int3 a, Int3 b)
	{
		return new Int3(
			VectorUtils.CrossX(a.X, a.Y, a.Z, b.X, b.Y, b.Z),
			VectorUtils.CrossY(a.X, a.Y, a.Z, b.X, b.Y, b.Z),
			VectorUtils.CrossZ(a.X, a.Y, a.Z, b.X, b.Y, b.Z));
	}

	public static int DistanceSquared(Int3 a, Int3 b)
	{
		return (a - b).LengthSquared;
	}

	public static double Distance(Int3 a, Int3 b)
	{
		return (a - b).Length;
	}

	public static Int3 Min(Int3 a, Int3 b)
	{
		return new Int3(VectorUtils.Min(a.X, b.X), VectorUtils.Min(a.Y, b.Y), VectorUtils.Min(a.Z, b.Z));
	}

	public static Int3 Max(Int3 a, Int3 b)
	{
		return new Int3(VectorUtils.Max(a.X, b.X), VectorUtils.Max(a.Y, b.Y), VectorUtils.Max(a.Z, b.Z));
	}
}
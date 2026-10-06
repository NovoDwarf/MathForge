using MathForge.Attributes;

namespace MathForge.Vectors.Float;

[Vector(4)]
public readonly partial record struct Float4
{
	public Float4(float x, float y, float z, float w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}
	
	public Float4(double x, double y, double z, float w)
	{
		X = (float)x;
		Y = (float)y;
		Z = (float)z;
		W = (float)w;
	}

	public float X { get; }
	public float Y { get; }
	public float Z { get; }
	public float W { get; }

	public float Length => MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
	public float LengthSquared => X * X + Y * Y + Z * Z + W * W;
}
using MathForge.Attributes;

namespace MathForge.Vectors.Float;

[Vector(3)]
public readonly partial record struct Float3
{
	public Float3(float x, float y, float z)
	{
		X = x;
		Y = y;
		Z = z;
	}
	
	public Float3(double x, double y, double z)
	{
		X = (float)x;
		Y = (float)y;
		Z = (float)z;
	}

	public float X { get; }
	public float Y { get; }
	public float Z { get; }

	public float Length => MathF.Sqrt(X * X + Y * Y + Z * Z);
	public float LengthSquared => X * X + Y * Y + Z * Z;
}
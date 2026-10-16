using MathForge.Attributes;

namespace MathForge.LinearAlgebra.Vectors;

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
}
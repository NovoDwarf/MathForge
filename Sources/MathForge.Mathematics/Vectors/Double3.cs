using MathForge.Attributes;

namespace MathForge.Vectors;

[Vector(3)]
public readonly partial record struct Double3
{
	public Double3(double x, double y, double z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public double X { get; }
	public double Y { get; }
	public double Z { get; }
}
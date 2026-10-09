using MathForge.Attributes;

namespace MathForge.Vectors;

[Vector(4)]
public readonly partial record struct Double4
{
	public Double4(double x, double y, double z, double w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public double X { get; }
	public double Y { get; }
	public double Z { get; }
	public double W { get; }
}
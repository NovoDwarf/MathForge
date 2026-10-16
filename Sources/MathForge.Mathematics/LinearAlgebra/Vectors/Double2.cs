using MathForge.Attributes;

namespace MathForge.LinearAlgebra.Vectors;

[Vector(2)]
public readonly partial record struct Double2
{
	public Double2(double x, double y)
	{
		X = x;
		Y = y;
	}

	public double X { get; }
	public double Y { get; }
	
}
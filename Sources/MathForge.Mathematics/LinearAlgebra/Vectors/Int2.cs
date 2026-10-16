using MathForge.Attributes;

namespace MathForge.LinearAlgebra.Vectors;

[Vector(2)]
public readonly partial record struct Int2
{
	public Int2(int x, int y)
	{
		X = x;
		Y = y;
	}

	public int X { get; }
	public int Y { get; }
}
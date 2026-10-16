using MathForge.Attributes;

namespace MathForge.LinearAlgebra.Vectors;

[Vector(4)]
public readonly partial record struct Int4
{
	public Int4(int x, int y, int z, int w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public int X { get; }
	public int Y { get; }
	public int Z { get; }
	public int W { get; }
}
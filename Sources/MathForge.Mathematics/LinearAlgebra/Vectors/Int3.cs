using MathForge.Attributes;

namespace MathForge.LinearAlgebra.Vectors;

[Vector(3)]
public readonly partial record struct Int3
{
	public Int3(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public int X { get; }
	public int Y { get; }
	public int Z { get; }
}
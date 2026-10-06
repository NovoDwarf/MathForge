namespace MathForge.Vectors.Integer;

public readonly partial record struct Int4
{
	public static Int4 Zero => new(0, 0, 0, 0);
	public static Int4 One => new(1, 1, 1, 1);

	public static Int4 UnitX => new(1, 0, 0, 0);
	public static Int4 UnitY => new(0, 1, 0, 0);
	public static Int4 UnitZ => new(0, 0, 1, 0);
	public static Int4 UnitW => new(0, 0, 0, 1);
}
namespace MathForge.Vectors.Integer;

public readonly partial record struct Int4
{
	public override string ToString()
	{
		return $"({X}, {Y}, {Z}, {W})";
	}
}
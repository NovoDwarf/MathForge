namespace MathForge.Vectors.Integer;

public readonly partial record struct Int3
{
	public override string ToString()
	{
		return $"({X}, {Y}, {Z})";
	}
}
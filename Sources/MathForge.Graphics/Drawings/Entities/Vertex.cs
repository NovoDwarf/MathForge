using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Entities;

public sealed class Vertex
{
	public Vertex(Float2 position)
	{
		Position = position;
	}

	public Float2 Position { get; }

	public bool Visited { get; set; }
	public bool IsIntersection { get; set; }
	public bool IsEntry { get; set; }

	public Vertex? Next { get; set; }
	public Vertex? Prev { get; set; }
	public Vertex? Corresponding { get; set; }
}
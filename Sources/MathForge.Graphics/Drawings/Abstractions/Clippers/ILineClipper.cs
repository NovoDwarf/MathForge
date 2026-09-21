using MathForge.Graphics.Drawings.Entities;

namespace MathForge.Graphics.Drawings.Abstractions.Clippers;

public interface ILineClipper
{
	public Rect Rect { get; }

	public bool TryClip(LineSegment line, out LineSegment clipped);
}
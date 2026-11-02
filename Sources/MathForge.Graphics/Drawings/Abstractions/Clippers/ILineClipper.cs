using MathForge.Geometry.Primitives;
using MathForge.Graphics.Drawings.Entities;

namespace MathForge.Graphics.Drawings.Abstractions.Clippers;

public interface ILineClipper
{
	public Rect2 Rect { get; }

	public bool TryClip(Segment2 line, out Segment2 clipped);
}
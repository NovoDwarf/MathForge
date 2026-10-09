using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors;

namespace MathForge.Graphics.Drawings.Abstractions.Clippers;

public interface IPolygonClipper
{
	IEnumerable<IReadOnlyList<Float2>> Clip(IReadOnlyList<Float2> polygon, Rect clipRegion);
}
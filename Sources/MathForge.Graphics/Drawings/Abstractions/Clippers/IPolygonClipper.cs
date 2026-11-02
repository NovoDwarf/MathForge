using MathForge.Geometry.Primitives;
using MathForge.LinearAlgebra.Vectors;
using MathForge.Graphics.Drawings.Entities;


namespace MathForge.Graphics.Drawings.Abstractions.Clippers;

public interface IPolygonClipper
{
	IEnumerable<IReadOnlyList<Float2>> Clip(IReadOnlyList<Float2> polygon, Rect2 clipRegion);
}
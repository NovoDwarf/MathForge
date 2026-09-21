using MathForge.Geometry;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Abstractions.Fillers;

public interface IPolygonFiller
{
	public IEnumerable<Float3> Fill(IReadOnlyList<Float2> polygon, Size2 size);
}
using MathForge.LinearAlgebra.Vectors;
using MathForge.Geometry;
using MathForge.Geometry.Primitives;
using Float3 = MathForge.LinearAlgebra.Vectors.Float3;

namespace MathForge.Graphics.Drawings.Abstractions.Fillers;

public interface IPolygonFiller
{
	public IEnumerable<Float3> Fill(IReadOnlyList<Float2> polygon, Size2 size);
}
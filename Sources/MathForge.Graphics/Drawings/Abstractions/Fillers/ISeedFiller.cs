using MathForge.LinearAlgebra.Vectors;
using MathForge.Geometry;
using MathForge.Geometry.Primitives;
using Float3 = MathForge.LinearAlgebra.Vectors.Float3;


namespace MathForge.Graphics.Drawings.Abstractions.Fillers;

public interface ISeedFiller
{
	public IEnumerable<Float3> Fill(Float2 seed, Size2 size, Func<int, int, bool> isTarget);
}
using MathForge.Geometry;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Abstractions.Fillers;

public interface ISeedFiller
{
	public IEnumerable<Float3> Fill(Float2 seed, Size2 size, Func<int, int, bool> isTarget);
}
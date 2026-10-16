using MathForge.LinearAlgebra.Vectors;
using Float3 = MathForge.LinearAlgebra.Vectors.Float3;

namespace MathForge.Graphics.Drawings.Abstractions.Rasterizers;

public interface ILineRasterizer
{
	IEnumerable<Float3> Rasterize(Float2 start, Float2 end);
}
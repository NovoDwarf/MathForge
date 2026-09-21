using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Abstractions.Rasterizers;

public interface ILineRasterizer
{
	IEnumerable<Float3> Rasterize(Float2 start, Float2 end);
}
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Abstractions.Rasterizers;

public interface ICircleRasterizer
{
	public IEnumerable<Float3> Rasterize(int centerX, int centerY, int radius);
}
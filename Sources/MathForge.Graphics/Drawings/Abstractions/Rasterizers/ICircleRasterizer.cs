using MathForge.LinearAlgebra.Vectors;



using Float3 = MathForge.LinearAlgebra.Vectors.Float3;

namespace MathForge.Graphics.Drawings.Abstractions.Rasterizers;

public interface ICircleRasterizer
{
	public IEnumerable<Float3> Rasterize(int centerX, int centerY, int radius);
}
using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Lines;

public sealed class BresenhamLineRasterizer : ILineRasterizer
{
	private BresenhamLineRasterizer()
	{
	}

	public static BresenhamLineRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(Float2 start, Float2 end)
	{
		var dx = Math.Abs(end.X - start.X);
		var dy = Math.Abs(end.Y - start.Y);
		var sx = start.X < end.X ? 1 : -1;
		var sy = start.Y < end.Y ? 1 : -1;
		var error = dx - dy;

		var point = start;

		while (true)
		{
			yield return new Float3(point.X, point.Y, 1f);

			if (point == end)
				break;

			var e2 = 2 * error;

			if (e2 > -dy)
			{
				error -= dy;
				point = new Float2(point.X + sx, point.Y);
			}

			if (e2 < dx)
			{
				error += dx;
				point = new Float2(point.X, point.Y + sy);
			}
		}
	}
}
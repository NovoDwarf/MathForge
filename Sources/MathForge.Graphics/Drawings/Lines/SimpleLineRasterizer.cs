using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Lines;

public sealed class SimpleLineRasterizer : ILineRasterizer
{
	private SimpleLineRasterizer()
	{
	}

	public static SimpleLineRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(Float2 start, Float2 end)
	{
		var x0 = (int)start.X;
		var y0 = (int)start.Y;
		var x1 = (int)end.X;
		var y1 = (int)end.Y;

		var dx = x1 - x0;
		var dy = y1 - y0;

		if (dx == 0)
		{
			var step = y0 <= y1 ? 1 : -1;

			for (var y = y0; y != y1; y += step)
				yield return new Float3(x0, y, 1f);

			yield return new Float3(x1, y1, 1f);
			yield break;
		}

		var slope = (float)dy / dx;
		var stepX = dx > 0 ? 1 : -1;

		for (var x = x0; x != x1; x += stepX)
		{
			var y = y0 + slope * (x - x0);

			yield return new Float3(x, MathF.Round(y), 1f);
		}

		yield return new Float3(x1, y1, 1f);
	}
}
using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Circles;

public sealed class BresenhamCircleRasterizer : ICircleRasterizer
{
	private BresenhamCircleRasterizer()
	{
	}

	public static BresenhamCircleRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(int centerX, int centerY, int radius)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(radius);

		var x = 0;
		var y = radius;
		var error = 1 - radius;

		while (x <= y)
		{
			foreach (var point in Plot8(centerX, centerY, x, y)) yield return point;

			if (error < 0)
			{
				error += 2 * x + 3;
			}
			else
			{
				error += 2 * (x - y) + 5;
				y--;
			}

			x++;
		}
	}

	private static IEnumerable<Float3> Plot8(int cx, int cy, int x, int y)
	{
		yield return new Float3(cx + x, cy + y, 1f);
		yield return new Float3(cx - x, cy + y, 1f);
		yield return new Float3(cx + x, cy - y, 1f);
		yield return new Float3(cx - x, cy - y, 1f);

		if (x == 0 || x == y)
			yield break;

		yield return new Float3(cx + y, cy + x, 1f);
		yield return new Float3(cx - y, cy + x, 1f);
		yield return new Float3(cx + y, cy - x, 1f);
		yield return new Float3(cx - y, cy - x, 1f);
	}
}
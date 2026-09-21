using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Circles;

public sealed class WuCircleRasterizer : ICircleRasterizer
{
	private WuCircleRasterizer()
	{
	}

	public static WuCircleRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(int centerX, int centerY, int radius)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(radius);

		if (radius == 0)
		{
			yield return new Float3(centerX, centerY, 1f);
			yield break;
		}

		var limit = (int)MathF.Ceiling(radius / MathF.Sqrt(2f));

		for (var x = 0; x <= limit; x++)
		{
			var y = MathF.Sqrt(radius * radius - x * x);
			var yi = (int)MathF.Floor(y);
			var fraction = y - yi;

			foreach (var point in Plot8(centerX, centerY, x, yi, 1f - fraction))
				yield return point;

			foreach (var point in Plot8(centerX, centerY, x, yi + 1, fraction))
				yield return point;
		}
	}

	private static IEnumerable<Float3> Plot8(int centerX, int centerY, int x, int y, float coverage)
	{
		if (coverage <= 0f)
			yield break;

		yield return new Float3(centerX + x, centerY + y, coverage);
		yield return new Float3(centerX - x, centerY + y, coverage);
		yield return new Float3(centerX + x, centerY - y, coverage);
		yield return new Float3(centerX - x, centerY - y, coverage);

		if (x == y)
			yield break;

		yield return new Float3(centerX + y, centerY + x, coverage);
		yield return new Float3(centerX - y, centerY + x, coverage);
		yield return new Float3(centerX + y, centerY - x, coverage);
		yield return new Float3(centerX - y, centerY - x, coverage);
	}
}
using System.Runtime.CompilerServices;
using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Lines;

public sealed class WuLineRasterizer : ILineRasterizer
{
	private WuLineRasterizer()
	{
	}

	public static WuLineRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(Float2 start, Float2 end)
	{
		var x0 = start.X;
		var y0 = start.Y;
		var x1 = end.X;
		var y1 = end.Y;

		var steep = MathF.Abs(y1 - y0) > MathF.Abs(x1 - x0);

		if (steep)
		{
			(x0, y0) = (y0, x0);
			(x1, y1) = (y1, x1);
		}

		if (x0 > x1)
		{
			(x0, x1) = (x1, x0);
			(y0, y1) = (y1, y0);
		}

		var dx = x1 - x0;
		var dy = y1 - y0;
		var gradient = dx == 0f ? 1f : dy / dx;

		var xEnd = MathF.Round(x0);
		var yEnd = y0 + gradient * (xEnd - x0);
		var xGap = 1f - FractionalPart(x0 + 0.5f);

		var xPixel1 = (int)xEnd;
		var yPixel1 = (int)MathF.Floor(yEnd);

		if (steep)
		{
			yield return new Float3(yPixel1, xPixel1, (1f - FractionalPart(yEnd)) * xGap);
			yield return new Float3(yPixel1 + 1, xPixel1, FractionalPart(yEnd) * xGap);
		}
		else
		{
			yield return new Float3(xPixel1, yPixel1, (1f - FractionalPart(yEnd)) * xGap);
			yield return new Float3(xPixel1, yPixel1 + 1, FractionalPart(yEnd) * xGap);
		}

		var intery = yEnd + gradient;

		xEnd = MathF.Round(x1);
		yEnd = y1 + gradient * (xEnd - x1);
		xGap = FractionalPart(x1 + 0.5f);

		var xPixel2 = (int)xEnd;
		var yPixel2 = (int)MathF.Floor(yEnd);

		if (steep)
		{
			for (var x = xPixel1 + 1; x < xPixel2; x++)
			{
				var y = (int)MathF.Floor(intery);

				yield return new Float3(y, x, 1f - FractionalPart(intery));
				yield return new Float3(y + 1, x, FractionalPart(intery));

				intery += gradient;
			}

			yield return new Float3(yPixel2, xPixel2, (1f - FractionalPart(yEnd)) * xGap);
			yield return new Float3(yPixel2 + 1, xPixel2, FractionalPart(yEnd) * xGap);
		}
		else
		{
			for (var x = xPixel1 + 1; x < xPixel2; x++)
			{
				var y = (int)MathF.Floor(intery);

				yield return new Float3(x, y, 1f - FractionalPart(intery));
				yield return new Float3(x, y + 1, FractionalPart(intery));

				intery += gradient;
			}

			yield return new Float3(xPixel2, yPixel2, (1f - FractionalPart(yEnd)) * xGap);
			yield return new Float3(xPixel2, yPixel2 + 1, FractionalPart(yEnd) * xGap);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float FractionalPart(float value)
	{
		return value - MathF.Floor(value);
	}
}
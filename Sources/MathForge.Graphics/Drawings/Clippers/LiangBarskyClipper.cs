using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class LiangBarskyClipper : ILineClipper
{
	public LiangBarskyClipper(Rect rect)
	{
		Rect = rect;
	}

	public Rect Rect { get; }

	public bool TryClip(LineSegment line, out LineSegment clipped)
	{
		var x0 = line.Start.X;
		var y0 = line.Start.Y;
		var x1 = line.End.X;
		var y1 = line.End.Y;

		if (!Clip(ref x0, ref y0, ref x1, ref y1))
		{
			clipped = default;
			return false;
		}

		clipped = new LineSegment(new Float2(x0, y0), new Float2(x1, y1));
		return true;
	}

	private bool Clip(ref float x0, ref float y0, ref float x1, ref float y1)
	{
		var dx = x1 - x0;
		var dy = y1 - y0;

		var u1 = 0f;
		var u2 = 1f;

		var p = new[] { -dx, dx, -dy, dy };
		var q = new[] { x0 - Rect.MinX, Rect.MaxX - x0, y0 - Rect.MinY, Rect.MaxY - y0 };

		for (var i = 0; i < 4; i++)
		{
			if (p[i] == 0f)
			{
				if (q[i] < 0f)
					return false;

				continue;
			}

			var u = q[i] / p[i];

			if (p[i] < 0f)
				u1 = MathF.Max(u1, u);
			else
				u2 = MathF.Min(u2, u);

			if (u1 > u2)
				return false;
		}

		x0 += u1 * dx;
		y0 += u1 * dy;
		x1 += u2 * dx;
		y1 += u2 * dy;

		return true;
	}
}
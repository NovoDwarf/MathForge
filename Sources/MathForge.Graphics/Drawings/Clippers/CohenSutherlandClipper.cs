using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class CohenSutherlandClipper : ILineClipper
{
	public CohenSutherlandClipper(Rect rect)
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
		var code0 = ComputeOutCode(x0, y0);
		var code1 = ComputeOutCode(x1, y1);

		while (true)
		{
			if ((code0 | code1) == 0)
				return true;

			if ((code0 & code1) != 0)
				return false;

			var outCode = code0 != OutCode.Inside
				? code0
				: code1;

			float x;
			float y;

			if (outCode.HasFlag(OutCode.Top))
			{
				x = x0 + (x1 - x0) * (Rect.MaxY - y0) / (y1 - y0);
				y = Rect.MaxY;
			}
			else if (outCode.HasFlag(OutCode.Bottom))
			{
				x = x0 + (x1 - x0) * (Rect.MinY - y0) / (y1 - y0);
				y = Rect.MinY;
			}
			else if (outCode.HasFlag(OutCode.Right))
			{
				y = y0 + (y1 - y0) * (Rect.MaxX - x0) / (x1 - x0);
				x = Rect.MaxX;
			}
			else
			{
				y = y0 + (y1 - y0) * (Rect.MinX - x0) / (x1 - x0);
				x = Rect.MinX;
			}

			if (outCode == code0)
			{
				x0 = x;
				y0 = y;
				code0 = ComputeOutCode(x0, y0);
			}
			else
			{
				x1 = x;
				y1 = y;
				code1 = ComputeOutCode(x1, y1);
			}
		}
	}

	private OutCode ComputeOutCode(float x, float y)
	{
		var code = OutCode.Inside;

		if (x < Rect.MinX)
			code |= OutCode.Left;
		else if (x > Rect.MaxX)
			code |= OutCode.Right;

		if (y < Rect.MinY)
			code |= OutCode.Bottom;
		else if (y > Rect.MaxY)
			code |= OutCode.Top;

		return code;
	}
}
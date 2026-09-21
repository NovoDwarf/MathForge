using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class CyrusBeckClipper : ILineClipper
{
	public CyrusBeckClipper(Rect rect)
	{
		Rect = rect;
	}

	public Rect Rect { get; }

	public bool TryClip(LineSegment line, out LineSegment clipped)
	{
		return Clip(line, Rect, out clipped);
	}

	private static bool Clip(LineSegment line, Rect clipRect, out LineSegment clipped)
	{
		var direction = line.End - line.Start;

		var tEnter = 0f;
		var tLeave = 1f;

		List<Float3> boundaries =
		[
			new(line.Start.X, direction.X, clipRect.MinX),
			new(line.Start.X, direction.X, clipRect.MaxX),
			new(line.Start.Y, direction.Y, clipRect.MinY),
			new(line.Start.Y, direction.Y, clipRect.MaxY)
		];

		foreach (var (start, directionComponent, boundary) in boundaries)
		{
			var numerator = boundary - start;

			if (directionComponent == 0f)
			{
				if (numerator < 0f)
				{
					clipped = default;
					return false;
				}

				continue;
			}

			var t = numerator / directionComponent;

			if (directionComponent < 0f)
				tEnter = MathF.Max(tEnter, t);
			else
				tLeave = MathF.Min(tLeave, t);

			if (!(tEnter > tLeave))
				continue;

			clipped = default;
			return false;
		}

		clipped = new LineSegment(line.Start + tEnter * direction, line.Start + tLeave * direction);

		return true;
	}
}
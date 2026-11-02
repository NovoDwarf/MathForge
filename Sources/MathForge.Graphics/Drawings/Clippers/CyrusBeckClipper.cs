using MathForge.Geometry.Primitives;
using MathForge.LinearAlgebra.Vectors;
using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using Float3 = MathForge.LinearAlgebra.Vectors.Float3;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class CyrusBeckClipper : ILineClipper
{
    public CyrusBeckClipper(Rect2 rect)
    {
        Rect = rect;
    }

    public Rect2 Rect { get; }

    public bool TryClip(Segment2 line, out Segment2 clipped)
    {
        return Clip(line, Rect, out clipped);
    }

    private static bool Clip(Segment2 line, Rect2 clipRect, out Segment2 clipped)
    {
        var direction = line.End.Position - line.Start.Position;

        var tEnter = 0f;
        var tLeave = 1f;

        List<Float3> boundaries =
        [
            new(-line.Start.X, -direction.X, -clipRect.MinX),
            new(line.Start.X, direction.X, clipRect.MaxX),
            new(-line.Start.Y, -direction.Y, -clipRect.MinY),
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
            {
                tEnter = MathF.Max(tEnter, t);
            }
            else
            {
                tLeave = MathF.Min(tLeave, t);
            }

            if (!(tEnter > tLeave))
            {
                continue;
            }

            clipped = default;
            return false;
        }

        var clippedStart = line.Start.Position + tEnter * direction;
        var clippedEnd = line.Start.Position + tLeave * direction;
        clipped = new Segment2(new Point2(clippedStart.X, clippedStart.Y), new Point2(clippedEnd.X, clippedEnd.Y));

        return true;
    }
}

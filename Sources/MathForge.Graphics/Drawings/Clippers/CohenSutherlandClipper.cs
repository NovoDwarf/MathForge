using MathForge.Geometry.Primitives;
using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class CohenSutherlandClipper : ILineClipper
{
    public CohenSutherlandClipper(Rect2 rect)
    {
        Rect = rect;
    }

    public Rect2 Rect { get; }

    public bool TryClip(Segment2 line, out Segment2 clipped)
    {
        var start = line.Start;
        var end = line.End;
        if (!Clip(ref start, ref end))
        {
            clipped = default;
            return false;
        }

        clipped = new Segment2(start, end);

        return true;
    }

    private bool Clip(ref Point2 point0, ref Point2 point1)
    {
        var code0 = ComputeOutCode(point0.X, point0.Y);
        var code1 = ComputeOutCode(point1.X, point1.Y);

        while (true)
        {
            if ((code0 | code1) == 0)
            {
                return true;
            }

            if ((code0 & code1) != 0)
            {
                return false;
            }

            var outCode = code0 != OutCode.Inside
                ? code0
                : code1;

            float x;
            float y;

            if (outCode.HasFlag(OutCode.Top))
            {
                x = point0.X + (point1.X - point0.X) * (Rect.MaxY - point0.Y) / (point1.Y - point0.Y);
                y = Rect.MaxY;
            }
            else if (outCode.HasFlag(OutCode.Bottom))
            {
                x = point0.X + (point1.X - point0.X) * (Rect.MinY - point0.Y) / (point1.Y - point0.Y);
                y = Rect.MinY;
            }
            else if (outCode.HasFlag(OutCode.Right))
            {
                y = point0.Y + (point1.Y - point0.Y) * (Rect.MaxX - point0.X) / (point1.X - point0.X);
                x = Rect.MaxX;
            }
            else
            {
                y = point0.Y + (point1.Y - point0.Y) * (Rect.MinX - point0.X) / (point1.X - point0.X);
                x = Rect.MinX;
            }

            if (outCode == code0)
            {
                point0 = new Point2(x, y);
                code0 = ComputeOutCode(x, y);
            }
            else
            {
                point1 = new Point2(x, y);
                code1 = ComputeOutCode(x, y);
            }
        }
    }

    private OutCode ComputeOutCode(float x, float y)
    {
        var code = OutCode.Inside;

        if (x < Rect.MinX)
        {
            code |= OutCode.Left;
        }
        else if (x > Rect.MaxX)
        {
            code |= OutCode.Right;
        }

        if (y < Rect.MinY)
        {
            code |= OutCode.Bottom;
        }
        else if (y > Rect.MaxY)
        {
            code |= OutCode.Top;
        }

        return code;
    }
}

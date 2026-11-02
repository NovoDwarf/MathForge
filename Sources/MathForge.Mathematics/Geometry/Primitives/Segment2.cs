namespace MathForge.Geometry.Primitives;

public readonly record struct Segment2(Point2 Start, Point2 End)
{
    public static Segment2 FromSegment(Point2 start, Point2 end)
    {
        return new Segment2(start, end);
    }
}

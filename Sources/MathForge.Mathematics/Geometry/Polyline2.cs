namespace MathForge.Geometry;

public sealed class Polyline2
{
	public Polyline2(IReadOnlyList<Point2> points)
	{
		ArgumentNullException.ThrowIfNull(points);

		Points = points;
	}

	public IReadOnlyList<Point2> Points { get; }
}
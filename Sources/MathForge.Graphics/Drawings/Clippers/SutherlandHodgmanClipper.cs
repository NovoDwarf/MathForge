using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class SutherlandHodgmanClipper : IPolygonClipper
{
	public IEnumerable<IReadOnlyList<Float2>> Clip(IReadOnlyList<Float2> polygon, Rect clipRegion)
	{
		var output = polygon.ToList();

		output = ClipAgainstEdge(output, new Float2(clipRegion.MinX, 0f), Float2.Left);
		output = ClipAgainstEdge(output, new Float2(clipRegion.MaxX, 0f), Float2.Right);
		output = ClipAgainstEdge(output, new Float2(0f, clipRegion.MinY), Float2.Down);
		output = ClipAgainstEdge(output, new Float2(0f, clipRegion.MaxY), Float2.Up);

		return [output];
	}

	private static List<Float2> ClipAgainstEdge(IReadOnlyList<Float2> polygon, Float2 edgePoint, Float2 edgeNormal)
	{
		var output = new List<Float2>();

		if (polygon.Count == 0)
			return output;

		var previous = polygon[^1];
		var previousInside = Float2.IsInside(previous, edgePoint, edgeNormal);

		foreach (var current in polygon)
		{
			var currentInside = Float2.IsInside(current, edgePoint, edgeNormal);

			if (currentInside)
			{
				if (!previousInside) output.Add(Float2.ComputeIntersection(previous, current, edgePoint, edgeNormal));

				output.Add(current);
			}
			else if (previousInside)
			{
				output.Add(Float2.ComputeIntersection(previous, current, edgePoint, edgeNormal));
			}

			previous = current;
			previousInside = currentInside;
		}

		return output;
	}
}
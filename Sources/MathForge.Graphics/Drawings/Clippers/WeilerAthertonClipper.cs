using MathForge.Graphics.Drawings.Abstractions.Clippers;
using MathForge.Graphics.Drawings.Entities;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Clippers;

public sealed class WeilerAthertonClipper : IPolygonClipper
{
	public IEnumerable<IReadOnlyList<Float2>> Clip(IReadOnlyList<Float2> polygon, Rect clipRegion)
	{
		var clipPolygon = new[]
		{
			new Float2(clipRegion.MinX, clipRegion.MinY),
			new Float2(clipRegion.MaxX, clipRegion.MinY),
			new Float2(clipRegion.MaxX, clipRegion.MaxY),
			new Float2(clipRegion.MinX, clipRegion.MaxY)
		};

		var subjectList = CreateVertexList(polygon);
		var clipList = CreateVertexList(clipPolygon);

		if (subjectList.Count == 0)
			return [];

		FindIntersections(subjectList, clipList);
		MarkEntryExit(subjectList, clipPolygon);
		MarkEntryExit(clipList, polygon);

		return GenerateClippedPolygons(subjectList);
	}

	private static List<Vertex> CreateVertexList(IReadOnlyList<Float2> polygon)
	{
		var vertices = new List<Vertex>(polygon.Count);

		if (polygon.Count == 0)
			return vertices;

		Vertex? previous = null;
		Vertex? first = null;

		foreach (var point in polygon)
		{
			var vertex = new Vertex(point);

			if (previous is not null)
			{
				previous.Next = vertex;
				vertex.Prev = previous;
			}
			else
			{
				first = vertex;
			}

			previous = vertex;
			vertices.Add(vertex);
		}

		previous!.Next = first;
		first!.Prev = previous;

		return vertices;
	}

	private static void FindIntersections(List<Vertex> subject, List<Vertex> clip)
	{
		foreach (var subjectStart in subject)
		{
			var subjectEnd = subjectStart.Next!;

			foreach (var clipStart in clip)
			{
				var clipEnd = clipStart.Next!;

				if (!TryIntersect(subjectStart.Position, subjectEnd.Position, clipStart.Position, clipEnd.Position,
					    out var intersection))
					continue;

				var subjectIntersection = new Vertex(intersection) { IsIntersection = true };
				var clipIntersection = new Vertex(intersection) { IsIntersection = true };

				subjectIntersection.Corresponding = clipIntersection;
				clipIntersection.Corresponding = subjectIntersection;

				InsertVertex(subjectStart, subjectIntersection);
				InsertVertex(clipStart, clipIntersection);
			}
		}
	}

	private static void InsertVertex(Vertex start, Vertex vertex)
	{
		vertex.Next = start.Next;
		vertex.Prev = start;
		start.Next!.Prev = vertex;
		start.Next = vertex;
	}

	private static void MarkEntryExit(List<Vertex> polygon, IReadOnlyList<Float2> otherPolygon)
	{
		if (polygon.Count == 0)
			return;

		var inside = IsInside(polygon[0].Position, otherPolygon);

		foreach (var vertex in EnumerateVertices(polygon))
		{
			if (!vertex.IsIntersection)
				continue;

			vertex.IsEntry = !inside;
			inside = !inside;
		}
	}

	private static IEnumerable<Vertex> EnumerateVertices(List<Vertex> vertices)
	{
		if (vertices.Count == 0)
			yield break;

		var first = vertices[0];
		var current = first;

		do
		{
			yield return current;
			current = current.Next!;
		} while (current != first);
	}

	private static bool IsInside(Float2 point, IReadOnlyList<Float2> polygon)
	{
		var inside = false;

		for (var i = 0; i < polygon.Count; i++)
		{
			var a = polygon[i];
			var b = polygon[(i + 1) % polygon.Count];

			if (a.Y > point.Y == b.Y > point.Y)
				continue;

			var x = (b.X - a.X) * (point.Y - a.Y) / (b.Y - a.Y) + a.X;

			if (point.X < x)
				inside = !inside;
		}

		return inside;
	}

	private static IEnumerable<IReadOnlyList<Float2>> GenerateClippedPolygons(List<Vertex> subject)
	{
		foreach (var start in EnumerateVertices(subject))
		{
			if (!start.IsIntersection || !start.IsEntry || start.Visited)
				continue;

			var polygon = new List<Float2>();
			var current = start;
			var onSubject = true;

			while (true)
			{
				if (current.Visited && current == start)
					break;

				current.Visited = true;
				polygon.Add(current.Position);

				if (current.IsIntersection)
				{
					current = current.Corresponding!;
					onSubject = !onSubject;
				}

				current = current.Next!;

				if (current == start && onSubject)
					break;
			}

			if (polygon.Count > 0)
				yield return polygon;
		}
	}

	private static bool TryIntersect(Float2 p1, Float2 p2, Float2 q1, Float2 q2, out Float2 intersection)
	{
		var a1 = p2.Y - p1.Y;
		var b1 = p1.X - p2.X;
		var c1 = a1 * p1.X + b1 * p1.Y;

		var a2 = q2.Y - q1.Y;
		var b2 = q1.X - q2.X;
		var c2 = a2 * q1.X + b2 * q1.Y;

		var determinant = a1 * b2 - a2 * b1;

		if (MathF.Abs(determinant) < Float2.NormalizationEpsilon)
		{
			intersection = default;
			return false;
		}

		intersection = new Float2((b2 * c1 - b1 * c2) / determinant, (a1 * c2 - a2 * c1) / determinant);

		return IsBetween(p1, p2, intersection) && IsBetween(q1, q2, intersection);
	}

	private static bool IsBetween(Float2 a, Float2 b, Float2 point)
	{
		const float epsilon = Float2.NormalizationEpsilon;

		return point.X >= MathF.Min(a.X, b.X) - epsilon && point.X <= MathF.Max(a.X, b.X) + epsilon &&
		       point.Y >= MathF.Min(a.Y, b.Y) - epsilon && point.Y <= MathF.Max(a.Y, b.Y) + epsilon;
	}
}
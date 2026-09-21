using MathForge.Geometry;
using MathForge.Graphics.Drawings.Abstractions.Fillers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Fillers;

public sealed class ScanlineFiller : IPolygonFiller
{
	private ScanlineFiller()
	{
	}

	public static ScanlineFiller Default { get; } = new();

	public IEnumerable<Float3> Fill(IReadOnlyList<Float2> polygon, Size2 size)
	{
		var edgeTable = BuildEdgeTable(polygon, size.Height);
		var activeEdges = new List<Edge>();

		for (var y = 0; y < size.Height; y++)
		{
			activeEdges.AddRange(edgeTable[y]);
			activeEdges.RemoveAll(e => e.YMax == y);
			activeEdges.Sort((a, b) => a.X.CompareTo(b.X));

			for (var i = 0; i + 1 < activeEdges.Count; i += 2)
			{
				var xStart = (int)MathF.Round(activeEdges[i].X);
				var xEnd = (int)MathF.Round(activeEdges[i + 1].X);

				for (var x = xStart; x <= xEnd; x++)
					if (x >= 0 && x < size.Width)
						yield return new Float3(x, y, 1f);
			}

			for (var i = 0; i < activeEdges.Count; i++)
			{
				var edge = activeEdges[i];
				edge.X += edge.InvSlope;
				activeEdges[i] = edge;
			}
		}
	}

	private static List<Edge>[] BuildEdgeTable(IReadOnlyList<Float2> polygon, int height)
	{
		var edgeTable = new List<Edge>[height];

		for (var i = 0; i < height; i++)
			edgeTable[i] = [];

		var count = polygon.Count;

		for (var i = 0; i < count; i++)
		{
			var point1 = polygon[i];
			var point2 = polygon[(i + 1) % count];

			if ((int)point1.Y == (int)point2.Y)
				continue;

			var top = point1.Y < point2.Y ? point1 : point2;
			var bottom = point1.Y < point2.Y ? point2 : point1;

			var yTop = Math.Clamp((int)top.Y, 0, height - 1);
			var yBottom = Math.Clamp((int)bottom.Y, 0, height - 1);

			if (yTop >= height || yBottom < 0)
				continue;

			var invSlope = (bottom.X - top.X) / (bottom.Y - top.Y);

			edgeTable[yTop].Add(new Edge(top.X, invSlope, yBottom));
		}

		return edgeTable;
	}

	private record struct Edge(float X, float InvSlope, int YMax);
}
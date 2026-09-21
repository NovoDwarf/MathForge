using MathForge.Geometry;
using MathForge.Graphics.Drawings.Abstractions.Fillers;
using MathForge.Graphics.Neighborhoods;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Fillers;

public sealed class FloodFiller : ISeedFiller
{
	private FloodFiller()
	{
	}

	public static FloodFiller Default { get; } = new();

	public INeighborhood2D Neighborhood { get; set; } = MooreNeighborhood.Default;

	public IEnumerable<Float3> Fill(Float2 seed, Size2 size, Func<int, int, bool> isTarget)
	{
		var visited = new bool[size.Width, size.Height];
		var queue = new Queue<Float2>();

		queue.Enqueue(seed);

		while (queue.Count > 0)
		{
			var point = queue.Dequeue();

			var x = (int)point.X;
			var y = (int)point.Y;

			if (x < 0 || x >= size.Width || y < 0 || y >= size.Height)
				continue;

			if (visited[x, y])
				continue;

			if (!isTarget(x, y))
				continue;

			visited[x, y] = true;

			yield return new Float3(x, y, 1f);

			foreach (var offset in Neighborhood.Offsets2D)
				queue.Enqueue(point + offset);
		}
	}
}
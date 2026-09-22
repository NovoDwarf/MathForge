using MathForge.Geometry;
using MathForge.Graphics.Drawings.Abstractions.Fillers;
using MathForge.Neighborhoods;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Fillers;

public sealed class SeedFiller : ISeedFiller
{
	private SeedFiller()
	{
	}

	public static SeedFiller Default { get; } = new();

	public INeighborhood2D Neighborhood { get; set; } = MooreNeighborhood.Default;

	public IEnumerable<Float3> Fill(Float2 seed, Size2 size, Func<int, int, bool> isTarget)
	{
		var visited = new bool[size.Width, size.Height];
		var stack = new Stack<Float2>();

		stack.Push(seed);

		while (stack.Count > 0)
		{
			var point = stack.Pop();

			var x = (int)point.X;
			var y = (int)point.Y;

			if (x < 0 || x >= size.Width || y < 0 || y >= size.Height)
				continue;

			if (visited[x, y])
				continue;

			if (!isTarget(x, y))
				continue;

			var xLeft = x;

			while (xLeft >= 0 && !visited[xLeft, y] && isTarget(xLeft, y))
				xLeft--;

			xLeft++;

			var xRight = x;

			while (xRight < size.Width && !visited[xRight, y] && isTarget(xRight, y))
				xRight++;

			xRight--;

			for (var xi = xLeft; xi <= xRight; xi++)
			{
				visited[xi, y] = true;
				yield return new Float3(xi, y, 1f);
			}

			foreach (var offset in Neighborhood.Offsets2D)
			{
				var ny = y + offset.Y;

				if (ny < 0 || ny >= size.Height)
					continue;

				for (var xi = xLeft; xi <= xRight; xi++)
				{
					var nx = xi + offset.X;

					if (nx < 0 || nx >= size.Width)
						continue;

					if (!visited[nx, ny] && isTarget(nx, ny))
						stack.Push(new Float2(nx, ny));
				}
			}
		}
	}
}
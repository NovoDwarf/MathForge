using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Lines;

public sealed class DdaLineRasterizer : ILineRasterizer
{
	private DdaLineRasterizer()
	{
	}

	public static DdaLineRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(Float2 start, Float2 end)
	{
		var dx = end.X - start.X;
		var dy = end.Y - start.Y;

		var steps = (int)MathF.Max(MathF.Abs(dx), MathF.Abs(dy));

		if (steps == 0)
		{
			yield return new Float3(MathF.Round(start.X), MathF.Round(start.Y), 1f);
			yield break;
		}

		var xIncrement = dx / steps;
		var yIncrement = dy / steps;

		var x = start.X;
		var y = start.Y;

		for (var i = 0; i <= steps; i++)
		{
			yield return new Float3(MathF.Round(x), MathF.Round(y), 1f);

			x += xIncrement;
			y += yIncrement;
		}
	}
}
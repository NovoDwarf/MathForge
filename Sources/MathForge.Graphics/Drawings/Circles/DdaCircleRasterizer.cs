using MathForge.Graphics.Drawings.Abstractions.Rasterizers;
using MathForge.Vectors.Float;

namespace MathForge.Graphics.Drawings.Circles;

public sealed class DdaCircleRasterizer : ICircleRasterizer
{
	private DdaCircleRasterizer()
	{
	}

	public static DdaCircleRasterizer Default { get; } = new();

	public IEnumerable<Float3> Rasterize(int centerX, int centerY, int radius)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(radius);

		if (radius == 0)
		{
			yield return new Float3(centerX, centerY, 1f);
			yield break;
		}

		var circumference = 2d * Math.PI * radius;
		var steps = (int)Math.Ceiling(circumference);
		var dt = 2d * Math.PI / steps;

		for (var i = 0; i < steps; i++)
		{
			var t = i * dt;

			var x = centerX + radius * Math.Cos(t);
			var y = centerY + radius * Math.Sin(t);

			yield return new Float3((float)Math.Round(x), (float)Math.Round(y), 1f);
		}
	}
}
using MathForge.Vectors.Float;

namespace MathForge.Interpolations.Polynomial;

public static class LagrangeInterpolation
{
	public static float Interpolate(ReadOnlySpan<Float2> points, float x)
	{
		if (points.Length == 0)
			throw new ArgumentException("At least one point is required.");

		var result = 0f;

		for (var i = 0; i < points.Length; i++)
		{
			var basis = 1f;

			for (var j = 0; j < points.Length; j++)
			{
				if (i == j)
					continue;

				basis *= (x - points[j].X) / (points[i].X - points[j].X);
			}

			result += points[i].Y * basis;
		}

		return result;
	}
}
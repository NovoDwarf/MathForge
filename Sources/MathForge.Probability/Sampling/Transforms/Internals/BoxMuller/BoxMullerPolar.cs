using MathForge.Random.Generators;
using MathForge.Vectors.Float;

namespace MathForge.Sampling.Transforms.Internals.BoxMuller;

public static class BoxMullerPolar
{
	public static Float2 Transform(IRandom random)
	{
		double x, y, s;
		do
		{
			x = 2.0 * random.NextDouble() - 1.0;
			y = 2.0 * random.NextDouble() - 1.0;
			s = x * x + y * y;
		} while (s is >= 1.0 or 0.0);

		var factor = Math.Sqrt(-2.0 * Math.Log(s) / s);

		return new Float2(x * factor, y * factor);
	}
}
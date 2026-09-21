using MathForge.Random.Generators;
using MathForge.Vectors.Float;

namespace MathForge.Sampling.Transforms.Internals.BoxMuller;

public static class BoxMullerDirect
{
	public static Float2 Transform(IRandom random)
	{
		double u1, u2;

		do
		{
			u1 = random.NextDouble();
			u2 = random.NextDouble();
		} while (u1 == 0.0 || u2 == 0.0);

		var x = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
		var y = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

		return new Float2(x, y);
	}
}
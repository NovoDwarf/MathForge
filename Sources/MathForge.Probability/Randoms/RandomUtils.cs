using MathForge.Core.Utilities;
using MathForge.Numerical.Transforms;

namespace MathForge.Probability.Randoms;

/// <summary>
/// A collection of utility methods for random number generation.
/// </summary>
public static class RandomExtensions
{
	extension(RandomUtils)
	{
		public static double NextNormal()
		{
			return BoxMullerPolarTransform.Transform().u;
		}

		public static double NextNormal(double mean, double stdDev)
		{
			return mean + stdDev * RandomUtils.NextNormal();
		}
	}
}
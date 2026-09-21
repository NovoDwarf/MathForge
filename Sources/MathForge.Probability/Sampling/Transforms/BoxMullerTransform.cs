using MathForge.Random.Generators;
using MathForge.Sampling.Transforms.Internals.BoxMuller;
using MathForge.Vectors.Float;

namespace MathForge.Sampling.Transforms;

public static class BoxMullerTransform
{
	public static Float2 Transform(IRandom random, BoxMullerAlgorithm algorithm = BoxMullerAlgorithm.Auto)
	{
		return algorithm switch
		{
			BoxMullerAlgorithm.Auto => BoxMullerPolar.Transform(random),
			BoxMullerAlgorithm.Direct => BoxMullerDirect.Transform(random),
			BoxMullerAlgorithm.Polar => BoxMullerPolar.Transform(random),

			_ => throw new ArgumentOutOfRangeException(nameof(algorithm))
		};
	}
}

public enum BoxMullerAlgorithm
{
	Auto,
	Direct,
	Polar
}
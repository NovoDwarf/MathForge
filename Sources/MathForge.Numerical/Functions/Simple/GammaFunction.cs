using MathForge.Functions.Internals.Gamma;

namespace MathForge.Functions.Simple;

public enum GammaAlgorithm
{
	Lanczos,
	NumericalRecipes
}

public static class GammaFunction
{
	public static double Calculate(double x, GammaAlgorithm algorithm = GammaAlgorithm.Lanczos)
	{
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(x, 0);

		return algorithm switch
		{
			GammaAlgorithm.Lanczos => GammaLanczos.Calculate(x),
			GammaAlgorithm.NumericalRecipes => GammaNumericalRecipes.Calculate(x),

			_ => throw new ArgumentOutOfRangeException(nameof(algorithm))
		};
	}
}
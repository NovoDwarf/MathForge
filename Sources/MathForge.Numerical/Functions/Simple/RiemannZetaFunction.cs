using MathForge.Functions.Internals.RiemannZeta;

namespace MathForge.Functions.Simple;

public static class RiemannZetaFunction
{
	public static double Calculate(double s, RiemannZetaAlgorithm algorithm = RiemannZetaAlgorithm.Auto)
	{
		MathArgumentException.ThrowIfIsNaN(s);

		if (s == 1.0)
			return double.PositiveInfinity;

		return algorithm switch
		{
			RiemannZetaAlgorithm.Auto => CalculateAuto(s),
			RiemannZetaAlgorithm.EulerMaclaurin => RiemannZetaEulerMaclaurin.Calculate(s),
			_ => throw new ArgumentOutOfRangeException(nameof(algorithm))
		};
	}

	private static double CalculateAuto(double s)
	{
		if (s > 1.0)
			return RiemannZetaEulerMaclaurin.Calculate(s);

		return CalculateAnalyticContinuation(s);
	}

	private static double CalculateAnalyticContinuation(double s)
	{
		// TODO:
		throw new NotImplementedException();
	}
}
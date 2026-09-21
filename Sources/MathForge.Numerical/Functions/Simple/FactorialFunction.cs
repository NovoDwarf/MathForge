using MathForge.Functions.Internals.Factorial;

namespace MathForge.Functions.Simple;

public class FactorialFunction
{
	private static readonly double[] SmallFactorials =
	[
		1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880,
		3628800, 39916800, 479001600, 6227020800, 87178291200,
		1307674368000, 20922789888000, 355687428096000,
		6402373705728000, 121645100408832000
	];
	
	public static double Calculate(int n, FactorialAlgorithm algorithm = FactorialAlgorithm.Iterative)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(n);

		return algorithm switch
		{
			FactorialAlgorithm.Auto => CalculateAuto(n),
			FactorialAlgorithm.Iterative => FactorialIterative.Calculate(n),
			FactorialAlgorithm.Stirling => FactorialStirling.Calculate(n),
			_ => throw new ArgumentOutOfRangeException(nameof(algorithm))
		};
	}

	private static double CalculateAuto(int n)
	{
		if (TryGetSmallFactorial(n, out var f))
			return f;

		return FactorialStirling.Calculate(n);
	}
	
	private static bool TryGetSmallFactorial(int n, out double value)
	{
		if ((uint)n >= (uint)SmallFactorials.Length)
		{
			value = 0;
			return false;
		}

		value = SmallFactorials[n];
		return true;
	}
}

public enum FactorialAlgorithm
{
	Auto,
	Iterative,
	Stirling
}
namespace MathForge.Functions.Internals.Factorial;

internal static class FactorialStirling
{
	public static double Calculate(int n)
	{
		return Math.Sqrt(2 * Math.PI * n) * Math.Pow(n / Math.E, n) * (1 + 1.0 / (12 * n) + 1.0 / (288 * n * n));
	}
}
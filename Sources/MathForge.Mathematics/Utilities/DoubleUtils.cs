namespace MathForge.Utilities;

public static class DoubleUtils
{
	public const double AbsTol = 1e-12;
	public const double RelTol = 1e-10;

	public static bool Approximately(double a, double b)
		=> NumericUtils<double>.ApproximatelySmart(a, b, AbsTol, RelTol);
}
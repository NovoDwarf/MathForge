using MathForge.Utilities.Comparers;

namespace MathForge.Utilities;

public static class DoubleUtils
{
	public const double AbsTol = FloatingPointComparer.DoubleAbsoluteTolerance;
	public const double RelTol = FloatingPointComparer.DoubleRelativeTolerance;

	public static bool Approximately(double a, double b) => FloatingPointComparer.NearlyEquals(a, b);
}

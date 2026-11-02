namespace MathForge.Utilities.Comparers;

/// <summary>
/// Compares floating-point values using absolute and relative tolerances.
/// Approximate equality is not transitive and must not be used for hashing or sorting.
/// NaN is unordered; equal infinities and signed zeros compare equal.
/// </summary>
public static class FloatingPointComparer
{
    public const double DoubleAbsoluteTolerance = 1e-12;
    public const double DoubleRelativeTolerance = 1e-10;
    
    public const float SingleAbsoluteTolerance = 1e-6f;
    public const float SingleRelativeTolerance = 1e-5f;

    public static bool NearlyEquals(double a, double b,
        double absoluteTolerance = DoubleAbsoluteTolerance, double relativeTolerance = DoubleRelativeTolerance)
        => NumericUtils<double>.ApproximatelySmart(a, b, absoluteTolerance, relativeTolerance);

    public static bool NearlyEquals(float a, float b,
        float absoluteTolerance = SingleAbsoluteTolerance, float relativeTolerance = SingleRelativeTolerance)
        => NumericUtils<float>.ApproximatelySmart(a, b, absoluteTolerance, relativeTolerance);

    public static bool IsNearlyZero(double value, double absoluteTolerance = DoubleAbsoluteTolerance)
        => NearlyEquals(value, 0.0, absoluteTolerance, 0.0);

    public static bool IsNearlyZero(float value, float absoluteTolerance = SingleAbsoluteTolerance)
        => NearlyEquals(value, 0f, absoluteTolerance, 0f);

    public static bool LessThan(double a, double b,
        double absoluteTolerance = DoubleAbsoluteTolerance, double relativeTolerance = DoubleRelativeTolerance)
        => !NearlyEquals(a, b, absoluteTolerance, relativeTolerance) && a < b;

    public static bool LessThan(float a, float b,
        float absoluteTolerance = SingleAbsoluteTolerance, float relativeTolerance = SingleRelativeTolerance)
        => !NearlyEquals(a, b, absoluteTolerance, relativeTolerance) && a < b;

    public static bool GreaterThan(double a, double b,
        double absoluteTolerance = DoubleAbsoluteTolerance, double relativeTolerance = DoubleRelativeTolerance)
        => !NearlyEquals(a, b, absoluteTolerance, relativeTolerance) && a > b;

    public static bool GreaterThan(float a, float b,
        float absoluteTolerance = SingleAbsoluteTolerance, float relativeTolerance = SingleRelativeTolerance)
        => !NearlyEquals(a, b, absoluteTolerance, relativeTolerance) && a > b;

    public static bool LessThanOrEqual(double a, double b,
        double absoluteTolerance = DoubleAbsoluteTolerance, double relativeTolerance = DoubleRelativeTolerance)
        => NearlyEquals(a, b, absoluteTolerance, relativeTolerance) || a < b;

    public static bool LessThanOrEqual(float a, float b,
        float absoluteTolerance = SingleAbsoluteTolerance, float relativeTolerance = SingleRelativeTolerance)
        => NearlyEquals(a, b, absoluteTolerance, relativeTolerance) || a < b;

    public static bool GreaterThanOrEqual(double a, double b,
        double absoluteTolerance = DoubleAbsoluteTolerance, double relativeTolerance = DoubleRelativeTolerance)
        => NearlyEquals(a, b, absoluteTolerance, relativeTolerance) || a > b;

    public static bool GreaterThanOrEqual(float a, float b,
        float absoluteTolerance = SingleAbsoluteTolerance, float relativeTolerance = SingleRelativeTolerance)
        => NearlyEquals(a, b, absoluteTolerance, relativeTolerance) || a > b;
}

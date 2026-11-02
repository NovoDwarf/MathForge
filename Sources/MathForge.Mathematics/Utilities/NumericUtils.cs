using System.Numerics;

namespace MathForge.Utilities;

public static class NumericUtils<T> where T : INumber<T>
{
    public static bool ApproximatelySmart(T a, T b, T absoluteTolerance, T relativeTolerance)
    {
        ValidateTolerance(absoluteTolerance, nameof(absoluteTolerance));
        ValidateTolerance(relativeTolerance, nameof(relativeTolerance));

        if (a == b)
            return true;

        if (!T.IsFinite(a) || !T.IsFinite(b))
            return false;

        var difference = T.Zero;
        var hasFiniteDifference = false;

        try
        {
            difference = a >= b ? checked(a - b) : checked(b - a);
            hasFiniteDifference = T.IsFinite(difference);

            if (difference <= absoluteTolerance)
                return true;
        }
        catch (OverflowException)
        {
            // The exact difference exceeds the representable absolute tolerance.
        }

        if (relativeTolerance == T.Zero)
            return false;

        if (relativeTolerance >= T.One)
        {
            var sameSign = (a >= T.Zero && b >= T.Zero) || (a <= T.Zero && b <= T.Zero);

            if (sameSign || relativeTolerance >= T.One + T.One)
                return true;

            if (relativeTolerance == T.One)
                return false;
        }

        var scale = T.Max(T.Abs(a), T.Abs(b));
       
        return hasFiniteDifference
            ? difference / scale <= relativeTolerance
            : T.Abs(a / scale - b / scale) <= relativeTolerance;
    }

    public static bool Approximately(T a, T b, T tolerance)
    {
        return ApproximatelySmart(a, b, tolerance, T.Zero);
    }

    public static bool ApproximatelyZero(T value, T tolerance)
    {
        return Approximately(value, T.Zero, tolerance);
    }

    public static bool GreaterThan(T a, T b, T tolerance)
    {
        return !Approximately(a, b, tolerance) && a > b;
    }

    public static bool LessThan(T a, T b, T tolerance)
    {
        return !Approximately(a, b, tolerance) && a < b;
    }

    public static bool GreaterThanOrEqual(T a, T b, T tolerance)
    {
        return Approximately(a, b, tolerance) || a > b;
    }

    public static bool LessThanOrEqual(T a, T b, T tolerance)
    {
        return Approximately(a, b, tolerance) || a < b;
    }

    private static void ValidateTolerance(T tolerance, string parameterName)
    {
        if (!T.IsFinite(tolerance))
            throw new ArgumentOutOfRangeException(parameterName, "Tolerance must be finite and non-negative.");
      
        if (tolerance < T.Zero)
            throw new ArgumentOutOfRangeException(parameterName, "Tolerance must be finite and non-negative.");
    }
}

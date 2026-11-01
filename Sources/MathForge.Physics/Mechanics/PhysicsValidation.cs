using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

internal static class PhysicsValidation
{
    public static void RequireFinite(Double3 value, string parameterName)
    {
        if (!IsFinite(value))
            throw new ArgumentOutOfRangeException(parameterName, "All vector components must be finite.");
    }

    public static void RequireFinite(double value, string parameterName)
    {
        if (!double.IsFinite(value))
            throw new ArgumentOutOfRangeException(parameterName, "Value must be finite.");
    }

    public static void RequireNonNegative(double value, string parameterName)
    {
        RequireFinite(value, parameterName);
        ArgumentOutOfRangeException.ThrowIfNegative(value, parameterName);
    }

    public static void ValidateEvaluation(Mass mass, double time)
    {
        RequirePositive(mass.Kilograms, nameof(mass));
        RequireFinite(time, nameof(time));
    }

    public static void RequirePositive(double value, string parameterName)
    {
        RequireFinite(value, parameterName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, parameterName);
    }

    public static Double3 RequireFiniteResult(Double3 value)
    {
        if (!IsFinite(value))
            throw new ArithmeticException("The physical calculation produced a non-finite result.");
        
        return value;
    }

    private static bool IsFinite(Double3 value)
        => double.IsFinite(value.X) && double.IsFinite(value.Y) && double.IsFinite(value.Z);
}

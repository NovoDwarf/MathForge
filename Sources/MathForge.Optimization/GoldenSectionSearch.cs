using MathForge.Extensions;
using MathForge.Vectors;

namespace MathForge.Optimization;

public static class GoldenSectionSearch
{
    public static OptimizationResult Minimize(Func<double, double> objective, Range<double> bound, double tolerance = 1e-8, int maxIterations = 1000)
    {
        ArgumentNullException.ThrowIfNull(objective);
        
        MathArgumentException.ThrowIfNotFinite(bound.Start);
        MathArgumentException.ThrowIfNotFinite(bound.End);
        
        if (bound.Start >= bound.End)
            throw new ArgumentException("Lower bound must be less than upper bound.");

        if (!double.IsFinite(tolerance) || tolerance <= 0)
            throw new ArgumentOutOfRangeException(nameof(tolerance));
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
        
        var a = bound.Start;
        var b = bound.End;

        var x1 = b - MathConstants.InverseGoldenRatio * (b - a);
        var x2 = a + MathConstants.InverseGoldenRatio * (b - a);

        var f1 = objective(x1);
        var f2 = objective(x2);

        var iterations = 0;

        while (iterations < maxIterations && b - a > tolerance)
        {
            if (f1 <= f2)
            {
                b = x2;
                x2 = x1;
                f2 = f1;

                x1 = b - MathConstants.InverseGoldenRatio * (b - a);
                f1 = objective(x1);
            }
            else
            {
                a = x1;
                x1 = x2;
                f1 = f2;

                x2 = a + MathConstants.InverseGoldenRatio * (b - a);
                f2 = objective(x2);
            }

            iterations++;
        }

        var point = a + (b - a) / 2.0;
        var value = objective(point);

        return new OptimizationResult(point, value, iterations, b - a <= tolerance);
    }
}

public sealed record OptimizationResult(double Point, double Value, int Iterations, bool Converged);

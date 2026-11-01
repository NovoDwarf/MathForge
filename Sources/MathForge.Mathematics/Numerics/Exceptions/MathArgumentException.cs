using System.Numerics;
using System.Runtime.CompilerServices;
using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Numerics.Exceptions;

public static class MathArgumentException
{
    public static void ThrowIfNotFinite<TNumber>(TNumber number, string? paramName = null)
        where TNumber : INumber<TNumber>
    {
        ThrowIfNotFinite(number, paramName, "Value should be a finite.");
    }

    public static void ThrowIfNotFinite<TNumber>(TNumber number, string? paramName, string message)
        where TNumber : INumber<TNumber>
    {
        if (!TNumber.IsFinite(number))
        {
            throw new ArgumentOutOfRangeException(paramName ?? nameof(number), number, message);
        }
    }

    public static void ThrowIfNotInRange<TNumber>(TNumber number, Range<TNumber> range, string? paramName = null)
        where TNumber : INumber<TNumber>
    {
        ThrowIfNotInRange(number, range, paramName, "Value must belong to the specified range.");
    }

    public static void ThrowIfNotInRange<TNumber>(TNumber number, Range<TNumber> range, string? paramName, string message)
        where TNumber : INumber<TNumber>
    {
        if (!range.Contains(number))
        {
            throw new ArgumentOutOfRangeException(paramName ?? nameof(number), number, message);
        }
    }

    public static void ThrowIfIsNaN<TNumber>(TNumber number, string? paramName = null)
        where TNumber : INumber<TNumber>
    {
        ThrowIfIsNaN(number, paramName, "Value should be a number.");
    }

    public static void ThrowIfIsNaN<TNumber>(TNumber number, string? paramName, string message)
        where TNumber : INumber<TNumber>
    {
        if (TNumber.IsNaN(number))
        {
            throw new ArgumentOutOfRangeException(paramName ?? nameof(number), message);
        }
    }

    public static void ThrowIfIsInfinity<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must not be infinite.")
        where TNumber : INumber<TNumber>
    {
        if (TNumber.IsInfinity(number))
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfZero<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must not be zero.")
        where TNumber : INumber<TNumber>
    {
        if (number == TNumber.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfNegative<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must not be negative.")
        where TNumber : INumber<TNumber>
    {
        if (number < TNumber.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfNegativeOrZero<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must be positive.")
        where TNumber : INumber<TNumber>
    {
        if (number <= TNumber.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfPositive<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must not be positive.")
        where TNumber : INumber<TNumber>
    {
        if (number > TNumber.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfPositiveOrZero<TNumber>(TNumber number, [CallerArgumentExpression(nameof(number))] string? paramName = null, string message = "Value must be negative.")
        where TNumber : INumber<TNumber>
    {
        if (number >= TNumber.Zero)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message);
        }
    }

    public static void ThrowIfLessThan<TNumber>(TNumber number, TNumber other, [CallerArgumentExpression(nameof(number))] string? paramName = null, string? message = null)
        where TNumber : INumber<TNumber>
    {
        if (number < other)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message ?? $"Value must be greater than or equal to {other}.");
        }
    }

    public static void ThrowIfLessThanOrEqual<TNumber>(TNumber number, TNumber other, [CallerArgumentExpression(nameof(number))] string? paramName = null, string? message = null)
        where TNumber : INumber<TNumber>
    {
        if (number <= other)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message ?? $"Value must be greater than {other}.");
        }
    }

    public static void ThrowIfGreaterThan<TNumber>(TNumber number, TNumber other, [CallerArgumentExpression(nameof(number))] string? paramName = null, string? message = null)
        where TNumber : INumber<TNumber>
    {
        if (number > other)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message ?? $"Value must be less than or equal to {other}.");
        }
    }

    public static void ThrowIfGreaterThanOrEqual<TNumber>(TNumber number, TNumber other, [CallerArgumentExpression(nameof(number))] string? paramName = null, string? message = null)
        where TNumber : INumber<TNumber>
    {
        if (number >= other)
        {
            throw new ArgumentOutOfRangeException(paramName, number, message ?? $"Value must be less than {other}.");
        }
    }
}


using System.Numerics;
using MathForge.Vectors;

namespace MathForge;

public static class MathArgumentException
{
	public static void ThrowIfNotFinite<TNumber>(TNumber number, string? paramName = null) where TNumber : INumber<TNumber>
	{
		if (!TNumber.IsFinite(number))
			throw new ArgumentOutOfRangeException(paramName ?? nameof(number), number, "Value should be a finite.");
	}
	
	public static void ThrowIfNotInRange<TNumber>(TNumber number, Range<TNumber> range, string? paramName = null) where TNumber : INumber<TNumber>
	{
		if (!range.Contains(number))
			throw new ArgumentOutOfRangeException(paramName ?? nameof(number), number, "Value must belong to the specified range.");
	}
	
	public static void ThrowIfIsNaN<TNumber>(TNumber number, string? paramName = null) where TNumber : INumber<TNumber>
	{
		if (TNumber.IsNaN(number))
			throw new ArgumentOutOfRangeException(paramName ?? nameof(number), "Value should be a number.");
	}
}
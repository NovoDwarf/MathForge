using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Generators;

internal static class NumericTypeRules
{
	internal static bool TryGetResultType(ITypeSymbol left, ITypeSymbol right, out NumericTypeKind result)
	{
		return TryGetResultType(GetKind(left), GetKind(right), out result);
	}

	internal static NumericTypeKind GetKind(ITypeSymbol type)
	{
		return type.SpecialType switch
		{
			SpecialType.System_Int32 => NumericTypeKind.Int32,
			SpecialType.System_Int64 => NumericTypeKind.Int64,
			SpecialType.System_Single => NumericTypeKind.Float32,
			SpecialType.System_Double => NumericTypeKind.Float64,
			SpecialType.System_Decimal => NumericTypeKind.Decimal,
			_ => NumericTypeKind.Unknown
		};
	}
	
	private static bool TryGetResultType(NumericTypeKind left, NumericTypeKind right, out NumericTypeKind result)
	{
		result = NumericTypeKind.Unknown;

		if (left == NumericTypeKind.Unknown || right == NumericTypeKind.Unknown)
			return false;
		
		if (IsDecimal(left) != IsDecimal(right) && (IsFloatingPoint(left) || IsFloatingPoint(right)))
			return false;

		if (left == NumericTypeKind.Decimal || right == NumericTypeKind.Decimal)
		{
			result = NumericTypeKind.Decimal;
			return true;
		}

		if (left == NumericTypeKind.Float64 || right == NumericTypeKind.Float64)
		{
			result = NumericTypeKind.Float64;
			return true;
		}

		if (left == NumericTypeKind.Float32 || right == NumericTypeKind.Float32)
		{
			result = NumericTypeKind.Float32;
			return true;
		}

		if (left == NumericTypeKind.Int64 || right == NumericTypeKind.Int64)
		{
			result = NumericTypeKind.Int64;
			return true;
		}

		result = NumericTypeKind.Int32;
		return true;
	}

	public static string GetTypeName(NumericTypeKind kind)
	{
		return kind switch
		{
			NumericTypeKind.Int32 => "global::System.Int32",
			NumericTypeKind.Int64 => "global::System.Int64",
			NumericTypeKind.Float32 => "global::System.Single",
			NumericTypeKind.Float64 => "global::System.Double",
			NumericTypeKind.Decimal => "global::System.Decimal",
			_ => throw new ArgumentOutOfRangeException(nameof(kind))
		};
	}

	private static bool IsDecimal(NumericTypeKind kind) =>
		kind == NumericTypeKind.Decimal;

	private static bool IsFloatingPoint(NumericTypeKind kind) =>
		kind is NumericTypeKind.Float32 or NumericTypeKind.Float64;
}
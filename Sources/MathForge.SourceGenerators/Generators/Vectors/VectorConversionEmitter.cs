using System.Text;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Generators;

internal sealed class VectorConversionEmitter
{
	public string Emit(VectorModel source, VectorModel target)
	{
		if (source.Dimension == target.Dimension)
			throw new ArgumentException("Vector dimensions must differ.", nameof(target));

		if (!SymbolEqualityComparer.Default.Equals(source.ScalarType, target.ScalarType))
			throw new ArgumentException("Scalar types must match.", nameof(target));

		var sourceType = source.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		var targetType = target.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

		var arguments = string.Join(",\n        ", Enumerable.Range(0, target.Dimension).Select(i => i < source.Dimension
				                         ? $"value.{source.Coordinates[i].Name}"
				                         : $"default({source.ScalarType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"));

		return $"public static explicit operator {targetType}({sourceType} value) => new({arguments});";
	}
}
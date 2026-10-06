using MathForge.SourceGenerators.Vectors;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Generators;

internal sealed class VectorOperatorEmitter
{
    private static readonly string[] CoordinateNames = ["X", "Y", "Z", "W"];

    private static readonly string[] VectorOperators = ["+", "-", "*", "/"];

    private static readonly string[] ScalarOperators = ["*", "/"];

    private readonly VectorTypeRegistry _typeRegistry;

    public VectorOperatorEmitter(VectorTypeRegistry typeRegistry)
    {
        _typeRegistry = typeRegistry;
    }

    public string Emit(VectorModel vector, IReadOnlyCollection<VectorModel> vectors, IReadOnlyCollection<ITypeSymbol> scalarTypes)
    {
        var members = new List<string>();

        members.AddRange(EmitVectorOperators(vector, vectors));
        members.AddRange(EmitScalarOperators(vector, scalarTypes));

        return string.Join("\n\n", members);
    }

    private IEnumerable<string> EmitVectorOperators(VectorModel left, IEnumerable<VectorModel> vectors)
    {
        foreach (var right in vectors)
        {
            if (left.Dimension != right.Dimension)
                continue;

            if (!NumericTypeRules.TryGetResultType(left.ScalarType, right.ScalarType, out var kind))
                continue;

            if (!_typeRegistry.TryGet(left.Dimension, kind, out var result))
                continue;

            foreach (var op in VectorOperators)
                yield return EmitVectorOperator(left, right, result, op);
        }
    }

    private static string EmitVectorOperator(VectorModel left, VectorModel right, VectorModel result, string op)
    {
        var leftType = TypeName(left.Symbol);
        var rightType = TypeName(right.Symbol);
        var resultType = TypeName(result.Symbol);

        var coordinates = string.Join(",\n        ", Enumerable.Range(0, left.Dimension).Select(i => $"left.{CoordinateNames[i]} {op} right.{CoordinateNames[i]}"));

        return $"public static {resultType} operator {op}({leftType} left, {rightType} right) => new({coordinates});";
    }

    private IEnumerable<string> EmitScalarOperators(VectorModel vector, IEnumerable<ITypeSymbol> scalarTypes)
    {
        foreach (var scalarType in scalarTypes)
        {
            if (!NumericTypeRules.TryGetResultType(vector.ScalarType, scalarType, out var resultScalarType))
                continue;

            if (!_typeRegistry.TryGet(vector.Dimension, resultScalarType, out var result))
                continue;

            foreach (var op in ScalarOperators)
                yield return EmitVectorScalarOperator(vector, scalarType, result, op);

            yield return EmitScalarVectorMultiplication(vector, scalarType, result);
        }
    }

    private static string EmitVectorScalarOperator(VectorModel vector, ITypeSymbol scalarType, VectorModel result, string op)
    {
        var vectorTypeName = TypeName(vector.Symbol);
        var scalarTypeName = TypeName(scalarType);
        var resultTypeName = TypeName(result.Symbol);

        var coordinates = string.Join(
            ",\n        ",
            Enumerable.Range(0, vector.Dimension)
                .Select(i =>
                    $"value.{CoordinateNames[i]} {op} scalar"));

        return $"public static {resultTypeName} operator {op}({vectorTypeName} value, {scalarTypeName} scalar) => new({coordinates});";
    }

    private static string EmitScalarVectorMultiplication(
        VectorModel vector,
        ITypeSymbol scalarType,
        VectorModel result)
    {
        var vectorTypeName = TypeName(vector.Symbol);
        var scalarTypeName = TypeName(scalarType);
        var resultTypeName = TypeName(result.Symbol);

        var coordinates = string.Join(",\n        ", Enumerable.Range(0, vector.Dimension).Select(i => $"scalar * value.{CoordinateNames[i]}"));

        return $"public static {resultTypeName} operator *({scalarTypeName} scalar, {vectorTypeName} value) => new({coordinates});";
    }

    private static string TypeName(ITypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
}


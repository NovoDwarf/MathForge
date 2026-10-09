using MathForge.SourceGenerators.Generators;
using MathForge.SourceGenerators.Utilities;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Vectors.Emitters;

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

    public string Emit(VectorModel model, IReadOnlyCollection<VectorModel> models, IReadOnlyCollection<ITypeSymbol> scalarTypes)
    {
        var members = new List<string>();

        members.AddRange(EmitVectorOperators(model, models));
        members.AddRange(EmitScalarOperators(model, scalarTypes));

        return string.Join("\n\n", members);
    }

    private IEnumerable<string> EmitVectorOperators(VectorModel left, IEnumerable<VectorModel> models)
    {
        foreach (var right in models)
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
        var leftType = GeneratorUtils.GetTypeName(left.Symbol);
        var rightType = GeneratorUtils.GetTypeName(right.Symbol);
        var resultType = GeneratorUtils.GetTypeName(result.Symbol);

        var coordinates = string.Join(",\n        ", Enumerable.Range(0, left.Dimension).Select(i => $"left.{CoordinateNames[i]} {op} right.{CoordinateNames[i]}"));

        return $"public static {resultType} operator {op}({leftType} left, {rightType} right) => new({coordinates});";
    }

    private IEnumerable<string> EmitScalarOperators(VectorModel model, IEnumerable<ITypeSymbol> scalarTypes)
    {
        foreach (var scalarType in scalarTypes)
        {
            if (!NumericTypeRules.TryGetResultType(model.ScalarType, scalarType, out var resultScalarType))
                continue;

            if (!_typeRegistry.TryGet(model.Dimension, resultScalarType, out var result))
                continue;

            foreach (var op in ScalarOperators)
                yield return EmitVectorScalarOperator(model, scalarType, result, op);

            yield return EmitScalarVectorMultiplication(model, scalarType, result);
        }
    }

    private static string EmitVectorScalarOperator(VectorModel model, ITypeSymbol scalarType, VectorModel result, string op)
    {
        var vectorTypeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarTypeName = GeneratorUtils.GetTypeName(scalarType);
        var resultTypeName = GeneratorUtils.GetTypeName(result.Symbol);

        var coordinates = string.Join(",\n        ", Enumerable.Range(0, model.Dimension).Select(i => $"value.{CoordinateNames[i]} {op} scalar"));

        return $"public static {resultTypeName} operator {op}({vectorTypeName} value, {scalarTypeName} scalar) => new({coordinates});";
    }

    private static string EmitScalarVectorMultiplication(VectorModel model, ITypeSymbol scalarType, VectorModel result)
    {
        var vectorTypeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarTypeName = GeneratorUtils.GetTypeName(scalarType);
        var resultTypeName = GeneratorUtils.GetTypeName(result.Symbol);

        var coordinates = string.Join(",\n        ", Enumerable.Range(0, model.Dimension).Select(i => $"scalar * value.{CoordinateNames[i]}"));

        return $"public static {resultTypeName} operator *({scalarTypeName} scalar, {vectorTypeName} value) => new({coordinates});";
    }
}


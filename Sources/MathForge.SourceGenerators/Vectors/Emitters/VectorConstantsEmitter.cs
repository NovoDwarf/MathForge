using MathForge.SourceGenerators.Generators;
using MathForge.SourceGenerators.Utilities;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Vectors.Emitters;

internal sealed class VectorConstantsEmitter
{
    private static readonly string[] CoordinateNames = ["X", "Y", "Z", "W"];

    public string Emit(VectorModel model)
    {
        var members = new List<string>
        {
            EmitZero(model),
            EmitOne(model)
        };

        members.AddRange(EmitUnitVectors(model));

        if (model.Dimension == 2)
            members.AddRange(EmitDirections(model));

        return string.Join("\n\n", members);
    }

    private static string EmitZero(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var coordinates = string.Join(", ", Enumerable.Repeat($"default({GeneratorUtils.GetTypeName(model.ScalarType)})", model.Dimension));

        return $"public static {typeName} Zero => new({coordinates});";
    }

    private static string EmitOne(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);
        var coordinates = string.Join(", ", Enumerable.Repeat($"({scalarType})1", model.Dimension));

        return $"public static {typeName} One => new({coordinates});";
    }

    private static IEnumerable<string> EmitUnitVectors(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);

        for (var i = 0; i < model.Dimension; i++)
        {
            var i1 = i;
            var coordinates = Enumerable.Range(0, model.Dimension).Select(index => index == i1 ? $"({scalarType})1" : $"default({scalarType})");
            var values = string.Join(", ", coordinates);
            var name = CoordinateNames[i];

            yield return $"public static {typeName} Unit{name} => new({values});";
        }
    }

    private static IEnumerable<string> EmitDirections(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);

        yield return $"public static {typeName} Up => new(default({scalarType}), ({scalarType})1);";
        yield return $"public static {typeName} Down => new(default({scalarType}), ({scalarType})(-1));";
        yield return $"public static {typeName} Left => new(({scalarType})(-1), default({scalarType}));";
        yield return $"public static {typeName} Right => new(({scalarType})1, default({scalarType}));";
    }
}
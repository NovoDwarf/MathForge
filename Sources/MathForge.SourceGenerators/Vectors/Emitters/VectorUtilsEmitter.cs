using MathForge.SourceGenerators.Utilities;
using MathForge.SourceGenerators.Vectors.Entities;
using MathForge.SourceGenerators.Vectors.Enums;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Vectors.Emitters;

internal sealed class VectorUtilityEmitter
{
    public string Emit(VectorModel model)
    {
        var members = new[]
        {
            EmitEquals(model),
            EmitDeconstruct(model),
            EmitNearlyEquals(model),
            EmitGetHashCode(model),
            EmitToString(model)
        };

        return string.Join("\n\n", members);
    }

    private static string EmitEquals(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var conditions = string.Join(" && ", model.Coordinates.Select(coordinate => $"{coordinate.Name} == other.{coordinate.Name}"));

        return $"public bool Equals({typeName} other) => {conditions};";
    }

    private static string EmitDeconstruct(VectorModel model)
    {
        var parameters = string.Join(", ", model.Coordinates.Select(coordinate => $"out {GeneratorUtils.GetTypeName(coordinate.Type)} {coordinate.Name.ToLowerInvariant()}"));
        var assignments = string.Join("\n", model.Coordinates.Select(coordinate => $"        {coordinate.Name.ToLowerInvariant()} = {coordinate.Name};"));

        return $$"""
            public void Deconstruct({{parameters}})
            {
            {{assignments}}
            }
            """;
    }

    private static string EmitNearlyEquals(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);
        var kind = NumericTypeRules.GetKind(model.ScalarType);
        
        var invalidTolerance = kind is NumericTypeKind.Float32 or NumericTypeKind.Float64
            ? $"!{scalarType}.IsFinite(epsilon) || epsilon < 0"
            : "epsilon < 0";
        
        var conditions = string.Join(" && ", model.Coordinates.Select(coordinate =>
            kind is NumericTypeKind.Byte or NumericTypeKind.Int32 or NumericTypeKind.Int64
                ? $"global::System.Math.Abs((decimal){coordinate.Name} - other.{coordinate.Name}) <= epsilon"
                : $"global::MathForge.Utilities.NumericUtils<{scalarType}>.Approximately({coordinate.Name}, other.{coordinate.Name}, epsilon)"));

        return $$"""
            public bool NearlyEquals({{typeName}} other, {{scalarType}} epsilon = default)
            {
                if ({{invalidTolerance}})
                {
                    throw new global::System.ArgumentOutOfRangeException(nameof(epsilon));
                }

                return {{conditions}};
            }
            """;
    }

    private static string EmitGetHashCode(VectorModel model)
    {
        var arguments = string.Join(", ", model.Coordinates.Select(coordinate => coordinate.Name));

        return $"public override int GetHashCode() => global::System.HashCode.Combine({arguments});";
    }

    private static string EmitToString(VectorModel model)
    {
        var typeName = model.Symbol.Name;
        var coordinates = string.Join(", ", model.Coordinates.Select(coordinate => $"{{{coordinate.Name}}}"));
        
        return $"""public override string ToString() => $"{typeName}({coordinates})";""";
    }
}

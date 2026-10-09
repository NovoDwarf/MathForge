using MathForge.SourceGenerators.Generators;
using MathForge.SourceGenerators.Utilities;

namespace MathForge.SourceGenerators.Vectors.Emitters;

internal sealed class VectorMathEmitter
{
    public string Emit(VectorModel model, VectorTypeRegistry registry)
    {
        var members = new List<string>
        {
            EmitDot(model),
            EmitLengthSquared(model),
            EmitLength(model),
            EmitDistanceSquared(model),
            EmitDistance(model),
            EmitMin(model),
            EmitMax(model),
            EmitNormalize(model, registry)
        };

        if (model.Dimension == 3)
            members.Add(EmitCross(model));

        var kind = NumericTypeRules.GetKind(model.ScalarType);

        if (kind is NumericTypeKind.Float32 or NumericTypeKind.Float64)
            members.Add(EmitLerp(model));

        return string.Join("\n\n", members);
    }
    
    private static string EmitDot(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);

        var expression = string.Join(" + ", Enumerable.Range(0, model.Dimension).Select(i => $"a.{model.Coordinates[i].Name} * b.{model.Coordinates[i].Name}"));

        return $"public static {scalarType} Dot({typeName} a, {typeName} b) => {expression};";
    }

    private static string EmitLengthSquared(VectorModel model)
    {
        var lengthType = NumericTypeRules.GetLengthType(model);
        var expression = string.Join(" + ", model.Coordinates.Select(coordinate => $"(({lengthType}){coordinate.Name} * {coordinate.Name})"));

        return $"public {lengthType} LengthSquared => {expression};";
    }
    
    private static string EmitLength(VectorModel model)
    {
        var lengthType = NumericTypeRules.GetLengthType(model);

        return lengthType switch
        {
            "global::System.Single" => $"public {lengthType} Length => global::System.MathF.Sqrt(LengthSquared);",
            "global::System.Double" => $"public {lengthType} Length => global::System.Math.Sqrt(LengthSquared);",
            "global::System.Decimal" => $"public {lengthType} Length => (global::System.Decimal)global::System.Math.Sqrt((double)LengthSquared);",
            _ => throw new InvalidOperationException($"Unsupported length type: {lengthType}")
        };
    }
    
    private static string EmitDistanceSquared(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var lengthType = NumericTypeRules.GetLengthType(model);

        return $"public static {lengthType} DistanceSquared({typeName} a, {typeName} b) => (a - b).LengthSquared;";
    }

    private static string EmitDistance(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var lengthType = NumericTypeRules.GetLengthType(model);

        return lengthType switch
        {
            "global::System.Single" => $"public static {lengthType} Distance({typeName} a, {typeName} b) => global::System.MathF.Sqrt((a - b).LengthSquared);",
            "global::System.Double" => $"public static {lengthType} Distance({typeName} a, {typeName} b) => global::System.Math.Sqrt((a - b).LengthSquared);",
            "global::System.Decimal" => $"public static {lengthType} Distance({typeName} a, {typeName} b) => (global::System.Decimal)global::System.Math.Sqrt((double)(a - b).LengthSquared);",
            _ => throw new InvalidOperationException($"Unsupported length type: {lengthType}")
        };
    }

    private static string EmitMin(VectorModel model) => EmitComponentwise(model, "Min");

    private static string EmitMax(VectorModel model) => EmitComponentwise(model, "Max");

    private static string EmitComponentwise(VectorModel model, string operation)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var arguments = string.Join(",\n        ", Enumerable.Range(0, model.Dimension).Select(i => $"global::System.Math.{operation}(a.{model.Coordinates[i].Name}, b.{model.Coordinates[i].Name})"));

        return $"public static {typeName} {operation}({typeName} a, {typeName} b) => new({arguments});";
    }

    private static string EmitCross(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);

        return $"""
            public static {typeName} Cross({typeName} a, {typeName} b)
                => new(
                    ({scalarType})(a.Y * b.Z - a.Z * b.Y),
                    ({scalarType})(a.Z * b.X - a.X * b.Z),
                    ({scalarType})(a.X * b.Y - a.Y * b.X));
            """;
    }
    
    private static string EmitNormalize(VectorModel model, VectorTypeRegistry registry)
    {
        if (!registry.TryGet(model.Dimension, NumericTypeKind.Float32, out var floatVector))
            throw new InvalidOperationException($"Float vector with dimension {model.Dimension} is required for Normalize.");

        var resultType = GeneratorUtils.GetTypeName(floatVector.Symbol);
        var arguments = string.Join(", ", model.Coordinates.Select(static coordinate => $"(float){coordinate.Name} / length"));
        var zeroValues = string.Join(", ", model.Coordinates.Select(static _ => "0f"));

        return $$"""
                 public {{resultType}} Normalize()
                 {
                     var length = Length;

                     if (length == 0f)
                         return new {{resultType}}({{zeroValues}});

                     return new {{resultType}}({{arguments}});
                 }
                 """;
    }

    private static string EmitLerp(VectorModel model)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var scalarType = GeneratorUtils.GetTypeName(model.ScalarType);

        return $"public static {typeName} Lerp({typeName} a,{typeName} b,{scalarType} t) => a + (b - a) * t;";
    }


}

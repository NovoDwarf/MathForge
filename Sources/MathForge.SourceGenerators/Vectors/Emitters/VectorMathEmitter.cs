using MathForge.SourceGenerators.Utilities;
using MathForge.SourceGenerators.Vectors.Entities;
using MathForge.SourceGenerators.Vectors.Enums;

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
            EmitDistance(model, squared: true),
            EmitDistance(model, squared: false),
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
    
    private static string EmitDistance(VectorModel model, bool squared)
    {
        var typeName = GeneratorUtils.GetTypeName(model.Symbol);
        var lengthType = NumericTypeRules.GetLengthType(model);
        var name = squared ? "DistanceSquared" : "Distance";
       
        var differences = string.Join("\n", GetDistanceDifferences(model)
            .Select((difference, index) => $"    var delta{index} = {difference};"));
       
        var sum = string.Join(" + ", Enumerable.Range(0, model.Dimension)
            .Select(index => $"delta{index} * delta{index}"));
      
        var result = squared ? sum : $"global::System.Math.Sqrt((double)({sum}))";

        return $$"""
            public static {{lengthType}} {{name}}({{typeName}} a, {{typeName}} b)
            {
            {{differences}}
                return ({{lengthType}})({{result}});
            }
            """;
    }

    private static IEnumerable<string> GetDistanceDifferences(VectorModel model)
    {
        var kind = NumericTypeRules.GetKind(model.ScalarType);

        return model.Coordinates.Select(coordinate => kind switch
        {
            NumericTypeKind.Int64 => $"(double)((decimal)a.{coordinate.Name} - b.{coordinate.Name})",
            NumericTypeKind.Decimal => $"a.{coordinate.Name} - b.{coordinate.Name}",
            _ => $"(double)a.{coordinate.Name} - b.{coordinate.Name}"
        });
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
        var kind = NumericTypeRules.GetKind(model.ScalarType);
        var resultKind = kind is NumericTypeKind.Float64 or NumericTypeKind.Decimal
            ? NumericTypeKind.Float64
            : NumericTypeKind.Float32;

        if (!registry.TryGet(model.Dimension, resultKind, out var resultVector))
        {
            throw new InvalidOperationException($"{resultKind} vector with dimension {model.Dimension} is required for Normalize.");
        }

        var resultType = GeneratorUtils.GetTypeName(resultVector.Symbol);
        var scalarType = NumericTypeRules.GetTypeName(resultKind);
       
        var scale = model.Coordinates.Select(coordinate => $"global::System.Math.Abs((double){coordinate.Name})")
            .Aggregate((left, right) => $"global::System.Math.Max({left}, {right})");
        
        var scaledCoordinates = string.Join("\n", model.Coordinates.Select((coordinate, index) =>
            $"    var component{index} = (double){coordinate.Name} / scale;"));
      
        var lengthSquared = string.Join(" + ", Enumerable.Range(0, model.Dimension)
            .Select(index => $"component{index} * component{index}"));
      
        var arguments = string.Join(", ", Enumerable.Range(0, model.Dimension)
            .Select(index => $"({scalarType})(component{index} / length)"));

        return $$"""
                 public {{resultType}} Normalize()
                 {
                     var scale = {{scale}};

                     if (scale == 0.0)
                     {
                         return default;
                     }

                 {{scaledCoordinates}}
                     var length = global::System.Math.Sqrt({{lengthSquared}});

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

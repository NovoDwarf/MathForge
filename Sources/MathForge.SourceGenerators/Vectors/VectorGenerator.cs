using System.Collections.Immutable;
using MathForge.SourceGenerators.Vectors.Diagnostics;
using MathForge.SourceGenerators.Vectors.Emitters;
using MathForge.SourceGenerators.Vectors.Entities;
using MathForge.SourceGenerators.Vectors.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MathForge.SourceGenerators.Vectors;

[Generator(LanguageNames.CSharp)]
public sealed class VectorGenerator : IIncrementalGenerator
{
    private const string AttributeName = "MathForge.Attributes.VectorAttribute";

    private static readonly string[] CoordinateNames = ["X", "Y", "Z", "W"];

    private static readonly SpecialType[] SupportedTypes =
    [
        SpecialType.System_Byte,
        SpecialType.System_Int32,
        SpecialType.System_Int64,
        SpecialType.System_Single,
        SpecialType.System_Double,
        SpecialType.System_Decimal
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider
                                .CreateSyntaxProvider(static (node, _) => HasVectorAttributeSyntax(node), static (syntaxContext, _) => syntaxContext.SemanticModel.GetDeclaredSymbol((TypeDeclarationSyntax)syntaxContext.Node))
                                .Where(static symbol => symbol is not null)
                                .Select(static (symbol, _) => symbol!);
        
        context.RegisterSourceOutput(context.CompilationProvider.Combine(candidates.Collect()), static (productionContext, input) => Execute(productionContext, input.Left, input.Right));
    }

    private static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<INamedTypeSymbol> candidates)
    {
        var models = CollectModels(context, candidates);

        RemoveDuplicateShapes(context, models);

        if (models.Count == 0)
            return;

        var registry = new VectorTypeRegistry(models);
        
        var mathEmitter = new VectorMathEmitter();
        var operatorEmitter = new VectorOperatorEmitter(registry);
        var constantsEmitter = new VectorConstantsEmitter();
        var conversionEmitter = new VectorConversionEmitter();
        var utilityEmitter = new VectorUtilityEmitter();

        var scalarTypes = SupportedTypes.Select(compilation.GetSpecialType).Cast<ITypeSymbol>().ToArray();

        foreach (var model in models)
        {
            var math = mathEmitter.Emit(model, registry);
            var constants = constantsEmitter.Emit(model);
            var operators = operatorEmitter.Emit(model, models, scalarTypes);
            var utilities = utilityEmitter.Emit(model);

            var conversions = new List<string>();

            foreach (var target in models)
            {
                if (target.Dimension == model.Dimension)
                    continue;

                if (!SymbolEqualityComparer.Default.Equals(target.ScalarType, model.ScalarType))
                    continue;

                conversions.Add(conversionEmitter.Emit(model, target));
            }

            VectorSourceBuilder.AddSource(context, model, "Constants", constants);
            VectorSourceBuilder.AddSource(context, model, "Operators", operators);
            VectorSourceBuilder.AddSource(context, model, "Conversions", string.Join("\n\n", conversions));
            VectorSourceBuilder.AddSource(context, model, "Math", math);
            VectorSourceBuilder.AddSource(context, model, "Utilities", utilities);
        }
    }

    private static List<VectorModel> CollectModels(SourceProductionContext context, ImmutableArray<INamedTypeSymbol> candidates)
    {
        var models = new List<VectorModel>();
        var seenSymbols = new HashSet<string>(StringComparer.Ordinal);

        foreach (var symbol in candidates)
        {
            var symbolKey = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            if (!seenSymbols.Add(symbolKey))
                continue;

            var attribute = symbol.GetAttributes().FirstOrDefault(static attribute => attribute.AttributeClass?.ToDisplayString() == AttributeName);

            if (attribute is null)
            {
                var hasVectorAttribute = symbol.GetAttributes().Any(static attribute => attribute.AttributeClass?.Name == "VectorAttribute");

                if (hasVectorAttribute) 
                    context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.InvalidAttribute, symbol.Locations.FirstOrDefault(), symbol.Name));

                continue;
            }
            if (attribute.ConstructorArguments.Length != 1 || attribute.ConstructorArguments[0].Value is not int dimension)
                continue;

            var location = symbol.Locations.FirstOrDefault();

            if (dimension is < 1 or > 4)
            {
                context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.InvalidDimension, location, dimension));
                continue;
            }

            if (!IsValidDeclaration(symbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.InvalidDeclaration, location, symbol.Name)); 
                continue;
            }

            var coordinates = new List<IPropertySymbol>();
            var valid = true;

            for (var i = 0; i < dimension; i++)
            {
                var property = symbol.GetMembers(CoordinateNames[i]).OfType<IPropertySymbol>().FirstOrDefault();

                if (property is null || property.IsStatic || property.Parameters.Length != 0 || property.GetMethod is null || property.DeclaredAccessibility != Accessibility.Public)
                {
                    valid = false;
                    break;
                }

                coordinates.Add(property);
            }

            if (!valid || coordinates.Any(property => !SymbolEqualityComparer.Default.Equals(property.Type, coordinates[0].Type)))
            {
                context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.InvalidCoordinates, location, symbol.Name, string.Join(", ", CoordinateNames.Take(dimension))));
                continue;
            }

            var scalarType = coordinates[0].Type;

            if (NumericTypeRules.GetKind(scalarType) == NumericTypeKind.Unknown)
            {
                context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.InvalidNumericType, coordinates[0].Locations.FirstOrDefault() ?? location, scalarType.ToDisplayString()));
                continue;
            }

            models.Add(new VectorModel(symbol, dimension, scalarType, [.. coordinates]));
        }

        return models;
    }

    private static bool IsValidDeclaration(INamedTypeSymbol symbol)
    {
        if (symbol.TypeKind != TypeKind.Struct || !symbol.IsRecord || !symbol.IsReadOnly || symbol.Arity != 0 || symbol.ContainingType is not null || symbol.DeclaredAccessibility != Accessibility.Public)
            return false;

        var declarations = symbol.DeclaringSyntaxReferences
                                 .Select(reference => reference.GetSyntax())
                                 .OfType<TypeDeclarationSyntax>()
                                 .ToArray();

        return declarations.Length > 0 && declarations.All(declaration => declaration.Modifiers.Any(SyntaxKind.PartialKeyword));
    }
    
    private static bool HasVectorAttributeSyntax(SyntaxNode node)
    {
        if (node is not TypeDeclarationSyntax declaration)
            return false;

        return declaration.AttributeLists
                          .SelectMany(static list => list.Attributes)
                          .Any(static attribute =>
                          {
                              var name = attribute.Name.ToString();
                              var lastPart = name.Substring(name.LastIndexOf('.') + 1);
                              return lastPart is "Vector" or "VectorAttribute"; 
                          });
    }
    
    private static void RemoveDuplicateShapes(SourceProductionContext context, List<VectorModel> models)
    {
        var duplicateGroups = models
                              .GroupBy(model => (model.Dimension, Kind: NumericTypeRules.GetKind(model.ScalarType)))
                              .Where(static group => group.Count() > 1)
                              .ToArray();

        foreach (var group in duplicateGroups)
        {
            var duplicates = group.ToArray();
            var existing = duplicates[0];

            foreach (var duplicate in duplicates.Skip(1)) 
                context.ReportDiagnostic(Diagnostic.Create(VectorDiagnostics.DuplicateVectorShape, duplicate.Symbol.Locations.FirstOrDefault(), duplicate.Symbol.Name, duplicate.Dimension, NumericTypeRules.GetTypeName(group.Key.Kind), existing.Symbol.Name));

            foreach (var duplicate in duplicates)
                models.Remove(duplicate);
        }
    }
}

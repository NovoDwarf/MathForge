namespace MathForge.SourceGenerators.Generators;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public sealed class EntityParameterGenerator : IIncrementalGenerator
{
    private const string AttributeMetadataName = "MathForge.Core.EntityParameterAttribute";

    private static readonly DiagnosticDescriptor ClassMustBePartial = new(
        id: "MF001",
        title: "Entity must be partial",
        messageFormat: "Type '{0}' contains EntityParameter members and must be declared partial",
        category: "MathForge.Entity",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor UnsupportedMember = new(
        id: "MF002",
        title: "Unsupported entity parameter member",
        messageFormat: "Member '{0}' is not a writable field or property and cannot be used as an entity parameter",
        category: "MathForge.Entity",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var entities = context.SyntaxProvider
            .ForAttributeWithMetadataName(AttributeMetadataName,
                static (node, _) => node is PropertyDeclarationSyntax
                    or FieldDeclarationSyntax
                    or VariableDeclaratorSyntax,
                static (ctx, _) => GetEntity(ctx))
            .Where(static x => x is not null);

        context.RegisterSourceOutput(entities, 
            static (spc, entity) =>
            {
                if (entity is null)
                    return;

                GenerateSource(spc, entity);
            });
    }

    private static EntityInfo? GetEntity(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol.ContainingType is not null && context.TargetSymbol is IPropertySymbol or IFieldSymbol)
        {
            var type = context.TargetSymbol.ContainingType;
            var members = GetParameterMembers(type);

            return new EntityInfo(type, members);
        }

        return null;
    }

    private static ImmutableArray<EntityParameterInfo> GetParameterMembers(INamedTypeSymbol type)
    {
        var result = new List<EntityParameterInfo>();

        foreach (var member in type.GetMembers())
        {
            var attribute = member switch
            {
                IPropertySymbol property => GetAttribute(property),
                IFieldSymbol field => GetAttribute(field),
                _ => null
            };

            if (attribute is null)
                continue;

            switch (member)
            {
                case IPropertySymbol property:
                {
                    result.Add(property.SetMethod is null
                        ? new EntityParameterInfo(member, attribute, property.Type, isWritable: false)
                        : new EntityParameterInfo(member, attribute, property.Type, isWritable: true));

                    break;
                }

                case IFieldSymbol field:
                {
                    result.Add(new EntityParameterInfo(member, attribute, field.Type, isWritable: !field.IsReadOnly));
                    break;
                }
            }
        }

        return [.. result.OrderBy(static x => GetSourcePosition(x.Symbol))];
    }

    private static AttributeData? GetAttribute(ISymbol symbol)
    {
        return symbol.GetAttributes()
            .FirstOrDefault(static attribute => attribute.AttributeClass?.ToDisplayString() == AttributeMetadataName);
    }

    private static int GetSourcePosition(ISymbol symbol)
    {
        var location = symbol.Locations.FirstOrDefault();

        return location?.SourceSpan.Start ?? int.MaxValue;
    }

    private static void GenerateSource(
        SourceProductionContext context,
        EntityInfo entity)
    {
        var classSymbol = entity.Symbol;

        if (!IsPartial(classSymbol))
        {
            context.ReportDiagnostic(Diagnostic.Create(ClassMustBePartial, classSymbol.Locations.FirstOrDefault(), classSymbol.Name));

            return;
        }

        foreach (var parameter in entity.Parameters.Where(parameter => !parameter.IsWritable))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(UnsupportedMember, parameter.Symbol.Locations.FirstOrDefault(), parameter.Symbol.Name));
            
            return;
        }

        var source = GenerateClassSource(classSymbol, entity.Parameters);
        var hintName = $"{classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                     .Replace("global::", string.Empty)
                                     .Replace('.', '_')
                                     .Replace('<', '_')
                                     .Replace('>', '_')}_EntityParameters.g.cs";

        context.AddSource(hintName, SourceText.From(source, Encoding.UTF8));
    }

    private static bool IsPartial(INamedTypeSymbol symbol)
    {
        foreach (var syntaxReference in symbol.DeclaringSyntaxReferences)
        {
            if (syntaxReference.GetSyntax() is ClassDeclarationSyntax declaration && declaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                return true;
            }
        }

        return false;
    }

    private static string GenerateClassSource(
        INamedTypeSymbol classSymbol,
        ImmutableArray<EntityParameterInfo> parameters)
    {
        var namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace
                ? null
                : classSymbol.ContainingNamespace.ToDisplayString();

        var className = classSymbol.Name;

        var builder = new StringBuilder();

        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("#nullable enable");
        builder.AppendLine();

        if (namespaceName is not null)
        {
            builder.Append("namespace ")
                .Append(namespaceName)
                .AppendLine(";");
            builder.AppendLine();
        }

        builder.Append("partial class ")
            .Append(className)
            .AppendLine();
        builder.AppendLine("{");

        GenerateObjectSet(builder, parameters);
        builder.AppendLine();

        GenerateTypedSet(builder, parameters);

        builder.AppendLine("}");

        return builder.ToString();
    }

    private static void GenerateObjectSet(
        StringBuilder builder,
        ImmutableArray<EntityParameterInfo> parameters)
    {
        builder.AppendLine(
            "    public override void Set(params object[] parameters)");
        builder.AppendLine("    {");

        builder.Append("        if (parameters is null)")
            .AppendLine();

        builder.AppendLine(
            "            throw new global::System.ArgumentNullException(nameof(parameters));");

        builder.Append("        if (parameters.Length != ")
            .Append(parameters.Length)
            .AppendLine(")");

        builder.Append(
            "            throw new global::System.ArgumentException(");
        builder.Append(
            $"\"Expected {parameters.Length} parameters, got {{parameters.Length}}\", ");
        builder.AppendLine("nameof(parameters));");

        builder.AppendLine();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            builder.Append("        ")
                .Append(GetTypeName(parameter.Type))
                .Append(' ')
                .Append(GetParameterName(parameter, i))
                .Append(" = ")
                .Append(GenerateConversion(parameter.Type, $"parameters[{i}]"))
                .AppendLine(";");
        }

        builder.AppendLine();

        builder.Append("        Set(");

        for (var i = 0; i < parameters.Length; i++)
        {
            if (i > 0)
                builder.Append(", ");

            builder.Append(GetParameterName(parameters[i], i));
        }

        builder.AppendLine(");");
        builder.AppendLine("    }");
    }

    private static void GenerateTypedSet(StringBuilder builder, ImmutableArray<EntityParameterInfo> parameters)
    {
        builder.Append("    public void Set(");

        for (var i = 0; i < parameters.Length; i++)
        {
            if (i > 0)
                builder.Append(", ");

            var parameter = parameters[i];

            builder.Append(GetTypeName(parameter.Type))
                .Append(' ')
                .Append(GetParameterName(parameter, i));
        }

        builder.AppendLine(")");
        builder.AppendLine("    {");

        foreach (var parameter in parameters)
        {
            var parameterName = GetParameterName(
                parameter,
                parameters.IndexOf(parameter));

            builder.Append("        ")
                .Append(parameter.Symbol.Name)
                .Append(" = ")
                .Append(parameterName)
                .AppendLine(";");
        }

        builder.AppendLine();
        builder.AppendLine("        Validate();");
        builder.AppendLine("    }");
    }

    private static string GenerateConversion(
        ITypeSymbol type,
        string expression)
    {
        var nullable = type as INamedTypeSymbol;

        if (nullable?.OriginalDefinition.SpecialType ==
            SpecialType.System_Nullable_T)
        {
            var underlying = nullable.TypeArguments[0];

            return $"({GetTypeName(type)}){GenerateConversion(underlying, expression)}";
        }

        switch (type.SpecialType)
        {
            case SpecialType.System_Byte:
                return $"global::System.Convert.ToByte({expression})";

            case SpecialType.System_SByte:
                return $"global::System.Convert.ToSByte({expression})";

            case SpecialType.System_Int16:
                return $"global::System.Convert.ToInt16({expression})";

            case SpecialType.System_UInt16:
                return $"global::System.Convert.ToUInt16({expression})";

            case SpecialType.System_Int32:
                return $"global::System.Convert.ToInt32({expression})";

            case SpecialType.System_UInt32:
                return $"global::System.Convert.ToUInt32({expression})";

            case SpecialType.System_Int64:
                return $"global::System.Convert.ToInt64({expression})";

            case SpecialType.System_UInt64:
                return $"global::System.Convert.ToUInt64({expression})";

            case SpecialType.System_Single:
                return $"global::System.Convert.ToSingle({expression})";

            case SpecialType.System_Double:
                return $"global::System.Convert.ToDouble({expression})";

            case SpecialType.System_Decimal:
                return $"global::System.Convert.ToDecimal({expression})";

            case SpecialType.System_Boolean:
                return $"global::System.Convert.ToBoolean({expression})";

            case SpecialType.System_String:
                return $"global::System.Convert.ToString({expression})!";

            case SpecialType.System_Char:
                return $"global::System.Convert.ToChar({expression})";
        }

        if (type.TypeKind == TypeKind.Enum)
        {
            return $"({GetTypeName(type)})global::System.Convert.ToInt32({expression})";
        }

        return $"({GetTypeName(type)}){expression}";
    }

    private static string GetTypeName(ITypeSymbol type)
    {
        return type.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat);
    }

    private static string GetParameterName(
        EntityParameterInfo parameter,
        int index)
    {
        var name = parameter.Symbol.Name;

        if (SyntaxFacts.IsValidIdentifier(name))
            return char.ToLowerInvariant(name[0]) + name[1..];

        return $"parameter{index}";
    }

    private sealed class EntityInfo
    {
        public EntityInfo(
            INamedTypeSymbol symbol,
            ImmutableArray<EntityParameterInfo> parameters)
        {
            Symbol = symbol;
            Parameters = parameters;
        }

        public INamedTypeSymbol Symbol { get; }

        public ImmutableArray<EntityParameterInfo> Parameters { get; }
    }

    private sealed class EntityParameterInfo
    {
        public EntityParameterInfo(
            ISymbol symbol,
            AttributeData attribute,
            ITypeSymbol type,
            bool isWritable)
        {
            Symbol = symbol;
            Attribute = attribute;
            Type = type;
            IsWritable = isWritable;
        }

        public ISymbol Symbol { get; }

        public AttributeData Attribute { get; }

        public ITypeSymbol Type { get; }

        public bool IsWritable { get; }
    }
}
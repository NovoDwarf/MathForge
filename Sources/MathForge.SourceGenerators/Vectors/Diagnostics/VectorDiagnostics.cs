using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Diagnostics;

internal static class VectorDiagnostics
{
    private const string Category = "MathForge.VectorGenerator";

    public static readonly DiagnosticDescriptor InvalidAttribute = new(
        id: "MFV001",
        title: "Invalid vector attribute",
        messageFormat: "Type '{0}' uses an invalid VectorAttribute. Expected 'MathForge.Attributes.VectorAttribute'.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor InvalidDimension = new(
        id: "MFV002",
        title: "Invalid vector dimension",
        messageFormat: "Vector dimension must be between 1 and 4; received {0}",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor InvalidNumericType = new(
        id: "MFV003",
        title: "Invalid numeric type",
        messageFormat: "Coordinate type '{0}' is not supported",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor InvalidDeclaration = new(
        id: "MFV004",
        title: "Invalid vector declaration",
        messageFormat: "Type '{0}' must be a public, non-generic, top-level, readonly partial record struct",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor InvalidCoordinates = new(
        id: "MFV005",
        title: "Invalid vector coordinates",
        messageFormat: "Type '{0}' must declare public, readable coordinates {1} of the same type",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor DuplicateVectorShape = new(
        id: "MFV006",
        title: "Duplicate vector shape",
        messageFormat: "Vector '{0}' duplicates dimension {1} with scalar type '{2}', already registered by '{3}'",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor GenerationFailure = new(
        id: "MFV007",
        title: "Vector generation failed",
        messageFormat: "Failed to generate vector '{0}': {1}",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor VectorDiscovered = new(
        id: "MFV008",
        title: "Vector discovered",
        messageFormat: "Vector '{0}' discovered; dimension={1}, scalar type='{2}'",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor SourceGenerated = new(
        id: "MFV009",
        title: "Vector source generated",
        messageFormat: "Generated '{0}' for vector '{1}'",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);
}
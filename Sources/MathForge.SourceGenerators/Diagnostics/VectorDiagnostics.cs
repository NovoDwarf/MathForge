using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Diagnostics;

internal static class VectorDiagnostics
{
	private const string Category = "MathForge.VectorGenerator";

	public static readonly DiagnosticDescriptor InvalidDimension = new(
		id: "MFV001",
		title: "Invalid vector dimension",
		messageFormat: "Vector dimension must be between 1 and 4",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidNumericType = new(
		id: "MFV002",
		title: "Invalid numeric type",
		messageFormat: "Coordinate type '{0}' is not supported",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidDeclaration = new(
		id: "MFV003",
		title: "Invalid vector declaration",
		messageFormat: "Type '{0}' does not satisfy the vector generator requirements",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor InvalidCoordinates = new(
		id: "MFV004",
		title: "Invalid vector coordinates",
		messageFormat: "Type '{0}' must declare exactly {1} compatible coordinates",
		category: Category,
		defaultSeverity: DiagnosticSeverity.Error,
		isEnabledByDefault: true);
}
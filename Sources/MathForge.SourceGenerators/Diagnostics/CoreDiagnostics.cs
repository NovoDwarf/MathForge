using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Diagnostics;

public static class CoreDiagnostics
{
	public static readonly DiagnosticDescriptor ClassMustBePartial = new(
		"MFC001",
		"Entity must be partial",
		"Type '{0}' contains EntityParameter members and must be declared partial",
		"MathForge.Entity",
		DiagnosticSeverity.Error,
		true);

	public static readonly DiagnosticDescriptor UnsupportedMember = new(
		"MFC002",
		"Unsupported entity parameter member",
		"Member '{0}' is not a writable field or property and cannot be used as an entity parameter",
		"MathForge.Entity",
		DiagnosticSeverity.Error,
		true);
}
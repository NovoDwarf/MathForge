using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Diagnostics;

public static class PropertiesDiagnostics
{
	public static readonly DiagnosticDescriptor ErrorGeneratingProperty = new DiagnosticDescriptor(
			id: "MSP001",
			title: "Error generating properties",
			messageFormat: "Error generating properties for {0}: {1}",
			category: "Generation",
			defaultSeverity: DiagnosticSeverity.Error,
			isEnabledByDefault: true,
			description: "Error generating properties for {0}: {1}.");
	
}
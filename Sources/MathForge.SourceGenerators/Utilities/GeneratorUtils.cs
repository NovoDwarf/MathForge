using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Utilities;

public static  class GeneratorUtils
{
	private static readonly List<string> ClassSuffixes =
	[
		"Distribution"
	];

	internal static string GetTypeName(ITypeSymbol type)
		=> type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
	
	internal static string GetCleanName(string name)
	{
		return ClassSuffixes.Aggregate(name, (current, suffix) => current.Replace(suffix, string.Empty));
	}
	
	internal static bool HasValidSuffix(string className)
	{
		return ClassSuffixes.Any(suffix => className.EndsWith(suffix) && className.Length > suffix.Length);
	}
	
	internal static bool IsDistributionSubclass(INamedTypeSymbol? classSymbol)
	{
		return IsSubclass(classSymbol, "Distribution");
	}

	internal static bool IsEntitySubclass(INamedTypeSymbol? classSymbol)
	{
		return IsSubclass(classSymbol, "Entity");
	}

	internal static bool IsSubclass(INamedTypeSymbol? classSymbol, string className)
	{
		if (classSymbol == null) 
			return false;
		
		var baseType = classSymbol.BaseType;
		
		while (baseType != null)
		{
			if (string.Equals(baseType.Name, className, StringComparison.OrdinalIgnoreCase))
				return true;

			baseType = baseType.BaseType;
		}

		return false;
	}
}
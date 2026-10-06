using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MathForge.SourceGenerators.Generators;

internal sealed record VectorModel
{
	public VectorModel(INamedTypeSymbol Symbol,
		int Dimension,
		ITypeSymbol ScalarType,
		ImmutableArray<IPropertySymbol> Coordinates)
	{
		this.Symbol = Symbol;
		this.Dimension = Dimension;
		this.ScalarType = ScalarType;
		this.Coordinates = Coordinates;
	}

	public INamedTypeSymbol Symbol { get; }
	public int Dimension { get; }
	public ITypeSymbol ScalarType { get; }
	public ImmutableArray<IPropertySymbol> Coordinates { get; }

	public void Deconstruct(out INamedTypeSymbol symbol, out int dimension, out ITypeSymbol scalarType, out ImmutableArray<IPropertySymbol> coordinates)
	{
		symbol = Symbol;
		dimension = Dimension;
		scalarType = ScalarType;
		coordinates = Coordinates;
	}
}
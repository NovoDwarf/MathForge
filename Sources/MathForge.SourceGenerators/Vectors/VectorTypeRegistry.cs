using MathForge.SourceGenerators.Generators;

namespace MathForge.SourceGenerators.Vectors;

internal sealed class VectorTypeRegistry
{
	private readonly Dictionary<(int Dimension, NumericTypeKind Kind), VectorModel> _vectors = new();

	public VectorTypeRegistry(IEnumerable<VectorModel> vectors)
	{
		foreach (var vector in vectors)
			Add(vector);
	}

	public bool TryGet(int dimension, NumericTypeKind kind, out VectorModel model)
	{
		return _vectors.TryGetValue((dimension, kind), out model!);
	}

	private void Add(VectorModel vector)
	{
		if (vector.Dimension <= 0)
			throw new ArgumentOutOfRangeException(nameof(vector), vector.Dimension, "Vector dimension must be greater than zero.");

		var kind = NumericTypeRules.GetKind(vector.ScalarType);

		if (kind == NumericTypeKind.Unknown)
			throw new ArgumentException($"Unsupported vector scalar type '{vector.ScalarType}'.", nameof(vector));

		var key = (vector.Dimension, kind);

		if (_vectors.TryGetValue(key, out var existing))
		{
			throw new ArgumentException(
				$"A vector with dimension {vector.Dimension} and scalar type " +
				$"'{NumericTypeRules.GetTypeName(kind)}' is already registered " +
				$"as '{existing.Symbol.ToDisplayString()}'. Cannot register " +
				$"'{vector.Symbol.ToDisplayString()}'.",
				nameof(vector));
		}

		_vectors.Add(key, vector);
	}
}
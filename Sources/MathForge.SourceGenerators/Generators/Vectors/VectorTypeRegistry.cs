using MathForge.SourceGenerators.Generators;

namespace MathForge.SourceGenerators.Vectors;

internal sealed class VectorTypeRegistry
{
	private readonly Dictionary<(int Dimension, NumericTypeKind Kind), VectorModel> _vectors = new();

	public VectorTypeRegistry(IEnumerable<VectorModel> vectors)
	{
		foreach (var vector in vectors)
		{
			var kind = NumericTypeRules.GetKind(vector.ScalarType);

			if (kind == NumericTypeKind.Unknown)
				continue;

			// Duplicate dimension/type pairs should be diagnosed
			// during model validation, not silently overwritten here.
			_vectors.Add((vector.Dimension, kind), vector);
		}
	}

	public bool TryGet(
		int dimension,
		NumericTypeKind kind,
		out VectorModel model)
	{
		return _vectors.TryGetValue((dimension, kind), out model!);
	}
}
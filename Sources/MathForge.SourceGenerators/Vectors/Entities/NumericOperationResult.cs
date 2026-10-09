namespace MathForge.SourceGenerators.Generators;

internal readonly record struct NumericOperationResult
{
	public NumericOperationResult(bool isSupported, NumericTypeKind resultType)
	{
		IsSupported = isSupported;
		ResultType = resultType;
	}

	public bool IsSupported { get; }
	public NumericTypeKind ResultType { get; }

	public void Deconstruct(out bool isSupported, out NumericTypeKind resultType)
	{
		isSupported = IsSupported;
		resultType = ResultType;
	}
}
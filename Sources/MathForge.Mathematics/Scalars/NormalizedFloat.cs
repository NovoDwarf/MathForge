namespace MathForge.Scalars;

public readonly record struct NormalizedFloat
{
	public float Value { get; }

	public NormalizedFloat(float value)
	{
		if (value is < 0f or > 1f)
			throw new ArgumentOutOfRangeException(nameof(value));

		Value = value;
	}
	
	public static NormalizedFloat Zero => new(0f);
	public static NormalizedFloat One => new(1f);

	public static implicit operator NormalizedFloat(float value) => new(value);
	public static implicit operator float(NormalizedFloat value) => value.Value;
}
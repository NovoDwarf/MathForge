namespace MathForge.Scalars;

public readonly record struct NFloat
{
	public float Value { get; }

	public NFloat(float value)
	{
		if (value is < 0f or > 1f)
			throw new ArgumentOutOfRangeException(nameof(value));

		Value = value;
	}
	
	public static NFloat Zero => new(0f);
	public static NFloat One => new(1f);

	public static implicit operator NFloat(float value) => new(value);
	public static implicit operator float(NFloat value) => value.Value;
}
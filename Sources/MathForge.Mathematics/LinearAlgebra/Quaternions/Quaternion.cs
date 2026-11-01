namespace MathForge.LinearAlgebra.Quaternions;

public readonly partial record struct Quaternion(float X, float Y, float Z, float W)
{
	public static Quaternion Identity => new(0f, 0f, 0f, 1f);

	public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

	public float Length => MathF.Sqrt(LengthSquared);

	public Quaternion Conjugated => new(-X, -Y, -Z, W);

	public Quaternion Normalized
	{
		get
		{
			var length = Length;

			if (length == 0f)
				throw new InvalidOperationException("Cannot normalize a zero quaternion.");

			return new Quaternion(X / length, Y / length, Z / length, W / length);
		}
	}
}

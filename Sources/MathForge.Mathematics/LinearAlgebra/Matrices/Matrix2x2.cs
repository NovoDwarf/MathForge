using System.Numerics;

namespace MathForge.LinearAlgebra.Matrices;

public readonly struct Matrix2x2<T> where T : INumber<T>
{
	public T M11 { get; }
	public T M12 { get; }
	public T M21 { get; }
	public T M22 { get; }

	public Matrix2x2(T m11, T m12, T m21, T m22)
	{
		M11 = m11;
		M12 = m12;
		M21 = m21;
		M22 = m22;
	}

	public static Matrix2x2<T> Zero => new(T.Zero, T.Zero, T.Zero, T.Zero);

	public static Matrix2x2<T> Identity => new(T.One, T.Zero, T.Zero, T.One);

	public T Determinant => M11 * M22 - M12 * M21;

	public Matrix2x2<T> Transpose() => new(M11, M21, M12, M22);

	public static Matrix2x2<T> operator +(Matrix2x2<T> a, Matrix2x2<T> b) =>
		new(a.M11 + b.M11, a.M12 + b.M12, a.M21 + b.M21, a.M22 + b.M22);

	public static Matrix2x2<T> operator -(Matrix2x2<T> a, Matrix2x2<T> b) =>
		new(a.M11 - b.M11, a.M12 - b.M12, a.M21 - b.M21, a.M22 - b.M22);

	public static Matrix2x2<T> operator *(Matrix2x2<T> a, Matrix2x2<T> b) =>
		new(
			a.M11 * b.M11 + a.M12 * b.M21,
			a.M11 * b.M12 + a.M12 * b.M22,
			a.M21 * b.M11 + a.M22 * b.M21,
			a.M21 * b.M12 + a.M22 * b.M22);

	public static Matrix2x2<T> operator *(Matrix2x2<T> matrix, T scalar) =>
		new(matrix.M11 * scalar, matrix.M12 * scalar,
			matrix.M21 * scalar, matrix.M22 * scalar);
}
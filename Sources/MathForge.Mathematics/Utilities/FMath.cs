using System.Numerics;
using System.Runtime.CompilerServices;
using MathForge.Fields;

namespace MathForge.Utilities;

/// <summary>
/// Field math class
/// </summary>
public static class FMath
{
	public static Field<T> Add<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(a.Width, a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = sourceA[i] + sourceB[i];

		return result;
	}

	public static Field<T> Subtract<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(a.Width, a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = sourceA[i] - sourceB[i];

		return result;
	}

	public static Field<T> Multiply<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(a.Width, a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = sourceA[i] * sourceB[i];

		return result;
	}

	public static Field<T> Divide<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(a.Width, a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = sourceA[i] / sourceB[i];

		return result;
	}

	public static Field<T> Scale<T>(Field<T> field, T value) where T : INumber<T>
	{
		var result = new Field<T>(field.Width, field.Height);
		var source = field.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = source[i] * value;

		return result;
	}

	public static Field<T> Min<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(
			a.Width,
			a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++)
		{
			destination[i] = T.Min(sourceA[i], sourceB[i]);
		}

		return result;
	}

	public static Field<T> Max<T>(Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(a, b);

		var result = new Field<T>(a.Width, a.Height);

		var sourceA = a.Values;
		var sourceB = b.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = T.Max(sourceA[i], sourceB[i]);

		return result;
	}

	public static Field<T> Map<T>(Field<T> field, Func<T, T> function)
	{
		ArgumentNullException.ThrowIfNull(function);

		var result = new Field<T>(field.Width, field.Height);

		var source = field.Values;
		var destination = result.Values;

		for (var i = 0; i < destination.Length; i++) 
			destination[i] = function(source[i]);

		return result;
	}

	public static void AddInto<T>(Field<T> destination, Field<T> a, Field<T> b) where T : INumber<T>
	{
		EnsureSameSize(destination, a);
		EnsureSameSize(destination, b);

		var output = destination.Values;
		var sourceA = a.Values;
		var sourceB = b.Values;

		for (var i = 0; i < output.Length; i++) 
			output[i] = sourceA[i] + sourceB[i];
	}

	public static void ScaleInto<T>(Field<T> destination, Field<T> source, T value)
		where T : INumber<T>
	{
		EnsureSameSize(destination, source);

		var output = destination.Values;
		var input = source.Values;

		for (var i = 0; i < output.Length; i++) 
			output[i] = input[i] * value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void EnsureSameSize<T>(Field<T> a, Field<T> b)
	{
		ArgumentNullException.ThrowIfNull(a);
		ArgumentNullException.ThrowIfNull(b);

		if (a.Width != b.Width || a.Height != b.Height)
			throw new ArgumentException("Fields must have the same dimensions.");
	}
}
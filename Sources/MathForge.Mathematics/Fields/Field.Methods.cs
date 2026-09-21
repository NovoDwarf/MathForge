using System.Buffers;
using System.Runtime.CompilerServices;

namespace MathForge.Fields;

public sealed partial class Field<T>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetUnchecked(int x, int y) => _values[y * Width + x];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetUnchecked(int x, int y, T value) => _values[y * Width + x] = value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetUnchecked(int index) => _values[index];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetUnchecked(int index, T value) => _values[index] = value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int Index(int x, int y) => y * Width + x;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int ClampX(int x) => Math.Clamp(x, 0, Width - 1);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int ClampY(int y) => Math.Clamp(y, 0, Height - 1);

	public static Field<T> Rent(int width, int height)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

		var length = checked(width * height);
		var buffer = ArrayPool<T>.Shared.Rent(length);

		Array.Clear(buffer, 0, length);

		return new Field<T>(width, height, buffer, isPooled: true);
	}

	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;

		if (_isPooled) 
			ArrayPool<T>.Shared.Return(_values);
	}
}
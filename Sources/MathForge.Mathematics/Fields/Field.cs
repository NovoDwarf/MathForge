using System.Runtime.CompilerServices;

namespace MathForge.Fields;

/// <summary>Owns a two-dimensional buffer, optionally rented from the shared array pool.</summary>
/// <remarks>
/// Not thread-safe. Spans obtained from Values must not be used after this instance is disposed.
/// </remarks>
public sealed partial class Field<T> : IDisposable
{
    private readonly T[] _values;
    private readonly bool _isPooled;

    private bool _disposed;

    public Field(int width, int height) : this(width, height, CreateBuffer(width, height), isPooled: false)
    {
    }

    private Field(int width, int height, T[] buffer, bool isPooled)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

        var length = checked(width * height);

        if (buffer.Length < length)
            throw new ArgumentException("Buffer is too small", nameof(buffer));

        Width = width;
        Height = height;
        Length = length;
        
        _values = buffer;
        _isPooled = isPooled;
    }

    public int Width { get; }

    public int Height { get; }

    public int Length { get; }

    public Span<T> Values
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return _values.AsSpan(0, Length);
        }
    }

    public T this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetUnchecked(Index(ClampX(x), ClampY(y)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => SetUnchecked(Index(ClampX(x), ClampY(y)), value);
    }

    private static T[] CreateBuffer(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        return new T[checked(width * height)];
    }
}

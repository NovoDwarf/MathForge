using System.Runtime.CompilerServices;

namespace MathForge.Fields;

public sealed partial class Field<T> : IDisposable
{
    private readonly T[] _values;
    private readonly bool _isPooled;

    private bool _disposed;

    public Field(int width, int height) : this(width, height, new T[checked(width * height)], isPooled: false) { }

    private Field(int width, int height, T[] buffer, bool isPooled)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

        var length = checked(width * height);

        if (buffer.Length < length)
            throw new ArgumentException("Buffer is too small.", nameof(buffer));

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
        get => _values.AsSpan(0, Length);
    }

    public T this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _values[Index(ClampX(x), ClampY(y))];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _values[Index(ClampX(x), ClampY(y))] = value;
    }
}
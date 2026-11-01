using System.Numerics;
using MathForge.Numerics.Exceptions;

namespace MathForge.LinearAlgebra.Vectors;

public readonly struct Range<TNumber> where TNumber : INumber<TNumber>
{
    public TNumber Start { get; }
    public TNumber End { get; }
    
    public BoundType StartType { get; }
    public BoundType EndType { get; }

    public bool IsEmpty => Start == End && (StartType == BoundType.Exclusive || EndType == BoundType.Exclusive);

    public static Range<TNumber> Empty => new(TNumber.Zero, TNumber.Zero, BoundType.Exclusive, BoundType.Exclusive);

    public Range(TNumber start, TNumber end, BoundType startType = BoundType.Inclusive, BoundType endType = BoundType.Inclusive)
    {
        MathArgumentException.ThrowIfIsNaN(start);
        MathArgumentException.ThrowIfIsNaN(end);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, end);

        if (!Enum.IsDefined(startType))
            throw new ArgumentOutOfRangeException(nameof(startType));

        if (!Enum.IsDefined(endType))
            throw new ArgumentOutOfRangeException(nameof(endType));

        Start = start;
        End = end;
        StartType = startType;
        EndType = endType;
    }

    public bool Contains(TNumber value)
    {
        if (TNumber.IsNaN(value) || IsEmpty)
            return false;

        var aboveStart = StartType == BoundType.Inclusive ? value >= Start : value > Start;
        var belowEnd = EndType == BoundType.Inclusive ? value <= End : value < End;
       
        return aboveStart && belowEnd;
    }

    /// <summary>
    /// Returns the value or the nearest included endpoint.
    /// Throws when the range is empty or the required endpoint is excluded:
    /// an open interval has no nearest real value at that boundary.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TNumber Clamp(TNumber value)
    {
        MathArgumentException.ThrowIfIsNaN(value);

        if (IsEmpty)
            throw new InvalidOperationException("Cannot clamp to an empty range.");
        
        if (Contains(value))
            return value;
        
        var beforeStart = value <= Start;
        var boundType = beforeStart ? StartType : EndType;

        if (boundType == BoundType.Exclusive)
            throw new InvalidOperationException("The nearest endpoint is excluded from the range.");
        
        return beforeStart ? Start : End;
    }

    public bool Intersects(Range<TNumber> range) => !Intersection(range).IsEmpty;
    
    /// <summary>
    /// Converts this range's endpoints to T using checked numeric conversions before intersecting.
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Intersects<T>(Range<T> range) where T : INumber<T>
    {
        return ConvertTo<T>().Intersects(range);
    }

    /// <summary>
    /// Converts this range's endpoints to T using checked numeric conversions before intersecting.
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Range<T> Intersection<T>(Range<T> range) where T : INumber<T>
    {
        return ConvertTo<T>().Intersection(range);
    }
    
    /// <summary>
    /// Converts this range's endpoints to T using checked numeric conversions before merging.
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Range<T>[] Union<T>(Range<T> range) where T : INumber<T>
    {
        return ConvertTo<T>().Union(range);
    }

    public Range<TNumber> Intersection(Range<TNumber> range)
    {
        if (IsEmpty || range.IsEmpty)
            return Empty;
        
        var start = TNumber.Max(Start, range.Start);
        var end = TNumber.Min(End, range.End);

        if (start > end)
            return Empty;
        
        var startType = Contains(start) && range.Contains(start) ? BoundType.Inclusive : BoundType.Exclusive;
        var endType = Contains(end) && range.Contains(end) ? BoundType.Inclusive : BoundType.Exclusive;
        
        return new Range<TNumber>(start, end, startType, endType);
    }

    public Range<TNumber>[] Union(Range<TNumber> range)
    {
        if (IsEmpty)
            return range.IsEmpty ? [] : [range];
        
        if (range.IsEmpty)
            return [this];
        
        var left = Start <= range.Start ? this : range;
        var right = Start <= range.Start ? range : this;
        var hasGap = left.End < right.Start || (left.End == right.Start && left.EndType == BoundType.Exclusive && right.StartType == BoundType.Exclusive);

        if (hasGap)
            return [left, right];
        
        var start = left.Start;
        var end = TNumber.Max(left.End, right.End);
       
        var startType = Contains(start) || range.Contains(start) ? BoundType.Inclusive : BoundType.Exclusive;
        var endType = Contains(end) || range.Contains(end) ? BoundType.Inclusive : BoundType.Exclusive;
      
        return [new Range<TNumber>(start, end, startType, endType)];
    }

    private Range<T> ConvertTo<T>() where T : INumber<T>
    {
        if (IsEmpty)
            return Range<T>.Empty;
        
        var start = T.CreateChecked(Start);
        var end = T.CreateChecked(End);

        if (TNumber.CreateChecked(start) != Start || TNumber.CreateChecked(end) != End)
            throw new InvalidOperationException("Range endpoints cannot be converted without loss.");
        
        return new Range<T>(start, end, StartType, EndType);
    }
}

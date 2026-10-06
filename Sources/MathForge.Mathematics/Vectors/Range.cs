using System.Numerics;

namespace MathForge.Vectors;

using System.Numerics;

public readonly struct Range<TNumber> where TNumber : INumber<TNumber>
{
	public TNumber Start { get; }

	public TNumber End { get; }

	public BoundType StartType { get; }

	public BoundType EndType { get; }

	public bool IsEmpty => Start == End && (StartType == BoundType.Exclusive || EndType == BoundType.Exclusive);

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

		var aboveStart = StartType == BoundType.Inclusive
			? value >= Start
			: value > Start;

		var belowEnd = EndType == BoundType.Inclusive
			? value <= End
			: value < End;

		return aboveStart && belowEnd;
	}
}

public enum BoundType
{
	Inclusive,
	Exclusive
}
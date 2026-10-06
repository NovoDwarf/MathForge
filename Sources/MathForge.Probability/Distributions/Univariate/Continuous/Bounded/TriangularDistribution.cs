using MathForge.Core.Attributes;
using MathForge.Core.Utilities;
using MathForge.Random.Generators;
using MathForge.Utilities;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continious", "Bounded")]
public partial class TriangularDistribution : Distribution
{
	public override double Expected => (MinimumI + MaximumI + ModeI) / 3.0;

	public override double Mean => (MinimumI + MaximumI + ModeI) / 3.0;

	public override double Median => GetMedian();

	public override double Mode => ModeI;

	public override double Variance => (Math.Pow(MinimumI, 2) + Math.Pow(MaximumI, 2) + Math.Pow(ModeI, 2) - MinimumI * MaximumI - MinimumI * ModeI - MaximumI * ModeI) / 18.0;

	public override double Skewness => GetSkewness();

	public override double Kurtosis => -0.6;
	
	public override double StandardDeviation => Math.Sqrt(Variance);

	public override double Minimum => MinimumI;

	public override double Maximum => MaximumI;
	
	public double LeftSlope => 2 / ((MaximumI - MinimumI) * (ModeI - MinimumI));
	
	public double RightSlope => 2 / ((MaximumI - MinimumI) * (MaximumI - ModeI));

	[EntityParameter(nameof(Minimum))]
	public double MinimumI { get; private set; } = 0;
	
	[EntityParameter(nameof(Maximum))]
	public double MaximumI { get; private set; } = 10;
	
	[EntityParameter(nameof(Mode))]
	public double ModeI { get; private set; } = 5;

	protected override void Validate()
	{
		if (MinimumI >= MaximumI)
			throw new ArgumentOutOfRangeException(nameof(MinimumI), "Min must be less than max");
		
		if (ModeI < MinimumI || ModeI > MaximumI)
			throw new ArgumentOutOfRangeException(nameof(ModeI), "Mode must be between min and max");
	}

	public override double Sample(IRandom random)
	{
		var u = random.NextDouble();
		var fc = (ModeI - MinimumI) / (MaximumI - MinimumI);

		return u < fc
			? MinimumI + Math.Sqrt(u * (MaximumI - MinimumI) * (ModeI - MinimumI))
			: MaximumI - Math.Sqrt((1 - u) * (MaximumI - MinimumI) * (MaximumI - ModeI));
	}

	public override double Quantile(double p)
	{
		return p; // TODO: impelement this
	}
	
	public override double ProbabilityDensity(double x)
	{
		if (x < MinimumI || x > MaximumI)
			return 0;

		if (x < ModeI)
			return 2 * (x - MinimumI) / ((MaximumI - MinimumI) * (ModeI - MinimumI));

		if (x > ModeI)
			return 2 * (MaximumI - x) / ((MaximumI - MinimumI) * (MaximumI - ModeI));
		
		return 2 / (MaximumI - MinimumI);
	}

	public override double CumulativeDistribution(double x)
	{
		if (x < MinimumI)
			return 0;
		
		if (x > MaximumI)
			return 1;

		if (x <= ModeI)
			return Math.Pow(x - MinimumI, 2) / ((MaximumI - MinimumI) * (ModeI - MinimumI));

		return 1 - Math.Pow(MaximumI - x, 2) / ((MaximumI - MinimumI) * (MaximumI - ModeI));
	}
	
	public override string ToString() => $"Triangular Distribution [Min = {MinimumI}, Max = {MaximumI}, Mode = {ModeI}]";

	public override bool Equals(object? obj)
	{
		return obj is TriangularDistribution other 
		       && DoubleUtils.Approximately(MinimumI, other.MinimumI) 
		       && DoubleUtils.Approximately(MaximumI, other.MaximumI) 
			 && DoubleUtils.Approximately(ModeI, other.ModeI);
	}

	public override int GetHashCode() => HashCode.Combine(MinimumI, MaximumI, ModeI);
	
	private double GetMedian()
	{
		var mid = (MinimumI + MaximumI) / 2.0;
		
		if (ModeI >= mid)
			return MinimumI + Math.Sqrt((MaximumI - MinimumI) * (ModeI - MinimumI) / 2.0);

		return MaximumI - Math.Sqrt((MaximumI - MinimumI) * (MaximumI - ModeI) / 2.0);
	}
	
	private double GetSkewness()
	{
		var numerator = Math.Sqrt(2) * (MinimumI + MaximumI - 2 * ModeI) * (2 * MinimumI - MaximumI - ModeI) * (MinimumI - 2 * MaximumI + ModeI);
		var denominator = 5 * Math.Pow(MinimumI * MinimumI + MaximumI * MaximumI + ModeI * ModeI - MinimumI * MaximumI - MinimumI * ModeI - MaximumI * ModeI, 1.5);
		
		return numerator / denominator;
	}
}
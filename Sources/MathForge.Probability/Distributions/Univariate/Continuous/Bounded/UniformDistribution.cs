using MathForge.Core.Attributes;
using MathForge.Core.Utilities;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continious", "Bounded")]
public partial class UniformDistribution : Distribution
{
	public override double Expected => (MinimumI + MaximumI) / 2.0;

	public override double Mean => (MinimumI + MaximumI) / 2.0;

	public override double Median => (MinimumI + MaximumI) / 2.0;

	public override double Mode => double.NaN;

	public override double Variance => Math.Pow(MaximumI - MinimumI, 2) / 12.0;

	public override double Skewness => 0;

	public override double Kurtosis => -6.0 / 5.0;

	public override double StandardDeviation => Math.Sqrt(Variance);

	public override double Minimum => MinimumI;

	public override double Maximum => MaximumI;

	[EntityParameter(nameof(Minimum))]
	public double MinimumI { get; private set; } = 0;
	
	[EntityParameter(nameof(Maximum))]
	public double MaximumI { get; private set; } = 10;

	protected override void Validate()
	{
		if (MinimumI >= MaximumI)
			throw new ArgumentOutOfRangeException(nameof(Minimum), "Minimum must be less than maximum");
	}

	public override double Sample(IRandom random)
	{
		return random.NextDouble(MinimumI, MaximumI);
	}

	public override double Quantile(double p)
	{
		return p; // TODO: impelement this
	}

	public override double ProbabilityDensity(double x) => x < MinimumI || x > MaximumI ? 0 : 1.0 / (MaximumI - MinimumI);

	public override double CumulativeDistribution(double x)
	{
		if (x < MinimumI)
			return 0;
		
		if (x > MaximumI)
			return 1;
		
		return (x - MinimumI) / (MaximumI - MinimumI);
	}
	
	public override string ToString() => $"Uniform Distribution [Min = {MinimumI}, Max = {MaximumI}]";

	public override bool Equals(object? obj) => obj is UniformDistribution other && MinimumI == other.MinimumI && MaximumI == other.MaximumI;

	public override int GetHashCode() => HashCode.Combine(MinimumI, MaximumI);
}
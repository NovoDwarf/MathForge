using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class ArcsinDistribution : Distribution
{
	public override double Expected => 0.5;
	
	public override double Mean => 0.5;
	
	public override double Median => 0.5;
	
	public override double Mode => double.NaN;
	
	public override double Variance => 0.125;
	
	public override double Skewness => 0.0;
	
	public override double Kurtosis => -1.5;
	
	public override double StandardDeviation => Math.Sqrt(Variance);
	
	public override double Minimum => 0.0;
	
	public override double Maximum => 1.0;

	public override double Sample(IRandom random)
	{
		ArgumentNullException.ThrowIfNull(random);

		return Quantile(random.NextDouble());
	}

	public override double Quantile(double p)
	{
		if (p < 0.0 || p > 1.0)
			throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

		return Math.Pow(Math.Sin(Math.PI * p / 2.0), 2.0);
	}

	public override double ProbabilityDensity(double x)
	{
		if (x <= 0.0 || x >= 1.0)
			return double.PositiveInfinity;

		return 1.0 / (Math.PI * Math.Sqrt(x * (1.0 - x)));
	}

	public override double CumulativeDistribution(double x)
	{
		if (x <= 0.0)
			return 0.0;

		if (x >= 1.0)
			return 1.0;

		return 2.0 /
		       Math.PI *
		       Math.Asin(Math.Sqrt(x));
	}

	public override string ToString()
	{
		return "Arcsin Distribution [Support = [0, 1]]";
	}

	public override bool Equals(object? obj)
	{
		return obj is ArcsinDistribution;
	}

	public override int GetHashCode()
	{
		return typeof(ArcsinDistribution).GetHashCode();
	}
	
	protected override void Validate() { }
}
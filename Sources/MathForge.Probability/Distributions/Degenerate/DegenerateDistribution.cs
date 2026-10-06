using System.ComponentModel.DataAnnotations;
using MathForge.Core.Attributes;
using MathForge.Core.Utilities;
using MathForge.Random.Generators;
using MathForge.Utilities;
using MathForge.Vectors;

namespace MathForge.Distributions.Degenerate;

[Categories("Distributions", "Degenerate")]
public sealed partial class DegenerateDistribution : Distribution
{
	public override double Expected => Constant;

	public override double Mean => Constant;

	public override double Median => Constant;

	public override double Mode => Constant;

	public override double Variance => 0;

	public override double Skewness => double.NaN;

	public override double Kurtosis => double.NaN;

	public override double StandardDeviation => 0;

	public override double Minimum => Constant;

	public override double Maximum => Constant;
	
	[Range(0, double.MaxValue)]
	[EntityParameter(nameof(Constant))]
	public double Constant { get; private set; } = 1;
	
	public override double Sample(IRandom random) => Constant;

	public override double Quantile(double p)
	{
		MathArgumentException.ThrowIfNotInRange(p, new Range<double>(0, 1));
		
		return Constant;
	}

	public override double ProbabilityDensity(double x) => DoubleUtils.Approximately(x, Constant) ? double.PositiveInfinity : 0;

	public override double CumulativeDistribution(double x) => x < Constant ? 0 : 1;

	public override string ToString() => $"Degenerate Distribution [Constant = {Constant}]";

	public override bool Equals(object? obj) => obj is DegenerateDistribution other && DoubleUtils.Approximately(Constant, other.Constant);

	public override int GetHashCode() => Id.GetHashCode();

	protected override void Validate()
	{
		MathArgumentException.ThrowIfIsNaN(Constant);
	}
}
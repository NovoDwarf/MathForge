using MathForge.Core.Attributes;
using MathForge.Core.Utilities;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;
using MathForge.Sampling.Transforms;

namespace MathForge.Distributions.Univariate.Continuous.Unbounded;

[Categories("Distributions", "Univariate", "Continious", "Unbounded")]
public partial class NormalDistribution : Distribution
{
	public override double Expected => MeanI;
	
	public override double Mean => MeanI;
	
	public override double Median => MeanI;
	
	public override double Mode => MeanI;
	
	public override double Variance => StandardDeviationI * StandardDeviationI;
	
	public override double Skewness => 0;
	
	public override double Kurtosis => 0;
	
	public override double StandardDeviation => StandardDeviationI;
	
	public override double Minimum => double.NegativeInfinity;
	
	public override double Maximum => double.PositiveInfinity;

	[EntityParameter(nameof(Mean))]
	public double MeanI { get; private set; } = 0;
	
	[EntityParameter(nameof(StandardDeviation))]
	public double StandardDeviationI { get; private set; } = 1;

	protected override void Validate()
	{
		if (StandardDeviation <= 0)
			throw new ArgumentOutOfRangeException(nameof(StandardDeviation), "Standard deviation must be positive");
	}

	public override double Sample(IRandom random) => MeanI + StandardDeviationI * random.Next();
	
	public override double ProbabilityDensity(double x)
	{
		var exponent = -0.5 * Math.Pow((x - MeanI) / StandardDeviationI, 2);
		
		return Math.Exp(exponent) / (StandardDeviationI * Math.Sqrt(2 * Math.PI));
	}

	public override double CumulativeDistribution(double x)
	{
		var z = (x - MeanI) / (StandardDeviationI * Math.Sqrt(2));
		
		return 0.5 * (1 + ErrorFunction.Calculate(z));
	}

	public override double Quantile(double p)
	{
		if (p <= 0 || p >= 1)
			throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1");

		return p < 0.5 ? MeanI - StandardDeviationI * InverseNormalCDF(1 - p) : MeanI + StandardDeviationI * InverseNormalCDF(p);
	}

	private static double InverseNormalCDF(double p)
	{
		if (p <= 0 || p >= 1)
			throw new ArgumentOutOfRangeException(nameof(p));

		double[] a = { -3.969683028665376e+01, 2.209460984245205e+02,
					  -2.759285104469687e+02, 1.383577518672690e+02,
					  -3.066479806614716e+01, 2.506628277459239e+00 };

		double[] b = { -5.447609879822406e+01, 1.615858368580409e+02,
					  -1.556989798598866e+02, 6.680131188771972e+01,
					  -1.328068155288572e+01 };

		double[] c = { -7.784894002430293e-03, -3.223964580411365e-01,
					  -2.400758277161838e+00, -2.549732539343734e+00,
					  4.374664141464968e+00, 2.938163982698783e+00 };

		double[] d = { 7.784695709041462e-03, 3.224671290700398e-01,
					  2.445134137142996e+00, 3.754408661907416e+00 };

		double q, r;

		if (p < 0.02425)
		{
			q = Math.Sqrt(-2 * Math.Log(p));
			return (((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
				   ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
		}

		if (p > 0.97575)
		{
			q = Math.Sqrt(-2 * Math.Log(1 - p));
			return -(((((c[0] * q + c[1]) * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) /
			       ((((d[0] * q + d[1]) * q + d[2]) * q + d[3]) * q + 1);
		}

		q = p - 0.5;
		r = q * q;
		
		return (((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * q /
		       (((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1);
	}

	public override string ToString() => $"Normal Distribution [Mean = {MeanI}, StdDev = {StandardDeviationI}]";

	public override bool Equals(object? obj) => obj is NormalDistribution other && MeanI == other.MeanI && StandardDeviationI == other.StandardDeviationI;

	public override int GetHashCode() => HashCode.Combine(MeanI, StandardDeviationI);
}
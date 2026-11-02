using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Infinite;

[Categories("Distributions", "Univariate", "Discrete", "Infinite")]
public partial class GaussKuzminDistribution : Distribution
{
    public override double Expected => double.PositiveInfinity;
    
    public override double Mean => double.PositiveInfinity;
    
    public override double Median => Quantile(0.5);
    
    public override double Mode => 1.0;
    
    public override double Variance => double.PositiveInfinity;
    
    public override double Skewness => double.NaN;
    
    public override double Kurtosis => double.NaN;
    
    public override double StandardDeviation => double.PositiveInfinity;
    
    public override double Minimum => 1.0;
    
    public override double Maximum => double.PositiveInfinity;

    public override double Sample(IRandom random)
    {
        var p = random.NextDouble();

        var low = 1;
        var high = 2;

        while (CumulativeDistribution(high) < p)
        {
            if (high > int.MaxValue / 2)
                return high;

            high *= 2;
        }

        while (low < high)
        {
            var middle = low + (high - low) / 2;

            if (CumulativeDistribution(middle) >= p)
                high = middle;
            else
                low = middle + 1;
        }

        return low;
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(
                nameof(p),
                "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return double.PositiveInfinity;
        
        var denominator = Math.Pow(2.0, 1.0 - p) - 1.0;

        if (denominator <= 0.0)
            return double.PositiveInfinity;

        var value = 1.0 / denominator - 1.0;

        return Math.Max(1.0, Math.Ceiling(value));
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < 1.0 || x != Math.Floor(x))
            return 0.0;

        return ProbabilityMass((int)x);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < 1.0)
            return 0.0;

        if (double.IsPositiveInfinity(x))
            return 1.0;

        var k = Math.Floor(x);

        return 1.0 -
               Math.Log(
                   (k + 2.0) / (k + 1.0),
                   2.0);
    }

    public double ProbabilityMass(int k)
    {
        if (k < 1)
            return 0.0;

        return Math.Log(1.0 + 1.0 / (k * (k + 2.0)), 2.0);
    }

    public override string ToString()
    {
        return "Gauss-Kuzmin Distribution";
    }

    public override bool Equals(object? obj)
    {
        return obj is GaussKuzminDistribution;
    }

    public override int GetHashCode()
    {
        return typeof(GaussKuzminDistribution).GetHashCode();
    }

    protected override void Validate()
    {
    }
}
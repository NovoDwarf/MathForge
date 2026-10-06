using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class WignerSemicircleDistribution : Distribution
{
    public override double Expected => 0.0;
    
    public override double Mean => 0.0;
    
    public override double Median => 0.0;
    
    public override double Mode => 0.0;
    
    public override double Variance => R * R / 4.0;
    
    public override double Skewness => 0.0;
    
    public override double Kurtosis => -1.0;
    
    public override double StandardDeviation => R / 2.0;
    
    public override double Minimum => -R;
    
    public override double Maximum => R;

    [EntityParameter(nameof(R))]
    public double R { get; private set; } = 1.0;

    public override double Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        while (true)
        {
            var x = 2.0 * random.NextDouble() - 1.0;
            var y = 2.0 * random.NextDouble() - 1.0;

            if (x * x + y * y > 1.0)
                continue;

            return R * x;
        }
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");
        }

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return Maximum;
        
        var low = -R;
        var high = R;

        for (var i = 0; i < 100; i++)
        {
            var middle = (low + high) / 2.0;

            if (CumulativeDistribution(middle) < p)
                low = middle;
            else
                high = middle;
        }

        return (low + high) / 2.0;
    }

    public override double ProbabilityDensity(double x)
    {
        if (x <= -R || x >= R)
            return 0.0;

        return 2.0 * Math.Sqrt(R * R - x * x) / (Math.PI * R * R);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x <= -R)
            return 0.0;

        if (x >= R)
            return 1.0;

        var normalized = x / R;
        var root = Math.Sqrt(1.0 - normalized * normalized);

        return 0.5 + (normalized * root + Math.Asin(normalized)) / Math.PI;
    }

    public override string ToString()
    {
        return $"Wigner Semicircle Distribution [R = {R:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is WignerSemicircleDistribution other && R.Equals(other.R);
    }

    public override int GetHashCode()
    {
        return R.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(R) || double.IsInfinity(R))
            throw new ArgumentException("R must be finite.", nameof(R));

        if (R <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(R), "R must be greater than 0.");
    }
}
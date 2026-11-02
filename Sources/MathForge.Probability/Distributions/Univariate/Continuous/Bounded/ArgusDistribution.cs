using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class ArgusDistribution : Distribution
{
    public override double Expected => GetExpected();
    
    public override double Mean => GetExpected();
    
    public override double Median => Quantile(0.5);
    
    public override double Mode => Chi / Math.Sqrt(2.0);
    
    public override double Variance => GetVariance();
    
    public override double Skewness => GetSkewness();
    
    public override double Kurtosis => GetKurtosis();
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    
    public override double Minimum => 0.0;
    
    public override double Maximum => Chi;

    [EntityParameter(nameof(Chi))]
    public double Chi { get; private set; } = 1.0;

    private double _normalizationConstant;

    public override double Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        while (true)
        {
            var x = Chi * random.NextDouble();
            var y = random.NextDouble();

            var acceptance = x * Math.Sqrt(1.0 - (x / Chi) * (x / Chi));

            if (y <= acceptance)
                return x;
        }
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return Maximum;

        var low = 0.0;
        var high = Chi;

        for (var i = 0; i < 100; i++)
        {
            var middle = (low + high) / 2.0;

            if (CumulativeDistribution(middle) >= p)
                high = middle;
            else
                low = middle;
        }

        return (low + high) / 2.0;
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < 0.0 || x > Chi)
            return 0.0;

        if (x == 0.0 || x == Chi)
            return 0.0;

        var ratio = x / Chi;

        return _normalizationConstant * x * Math.Sqrt(1.0 - ratio * ratio);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x <= 0.0)
            return 0.0;

        if (x >= Chi)
            return 1.0;
        
        var ratio = x / Chi;
        var u = 1.0 - ratio * ratio;

        return GetCumulativeFromU(u);
    }

    public override string ToString()
    {
        return $"Argus Distribution [Chi = {Chi:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is ArgusDistribution other && Chi.Equals(other.Chi);
    }

    public override int GetHashCode()
    {
        return Chi.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(Chi) || double.IsInfinity(Chi))
            throw new ArgumentException("Chi must be a finite number.", nameof(Chi));

        if (Chi <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Chi), "Chi must be greater than 0.");

        _normalizationConstant = 3.0 / (Chi * Chi * Chi);
    }

    private double GetExpected() => Chi * 3.0 * Math.PI / 16.0;

    private double GetVariance()
    {
        var mean = Mean;
        var secondMoment = 2.0 * Chi * Chi / 5.0;

        return secondMoment - mean * mean;
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var mean = Mean;

        var second = 2.0 * Chi * Chi / 5.0;
        var third = 3.0 * Math.PI * Chi * Chi * Chi / 32.0;
        var centralMoment = third
                            - 3.0 * mean * second
                            + 2.0 * Math.Pow(mean, 3.0);

        return centralMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var mean = Mean;

        var second = 2.0 * Chi * Chi / 5.0;
        var third = 3.0 * Math.PI * Chi * Chi * Chi / 32.0;
        var fourth = 8.0 * Chi * Chi * Chi * Chi / 35.0;
        var centralMoment = fourth
                            - 4.0 * mean * third
                            + 6.0 * mean * mean * second
                            - 3.0 * Math.Pow(mean, 4.0);

        return centralMoment /
               (variance * variance) - 3.0;
    }

    private double GetCumulativeFromU(double u)
    {
        return 1.0 - Math.Pow(u, 1.5);
    }
}
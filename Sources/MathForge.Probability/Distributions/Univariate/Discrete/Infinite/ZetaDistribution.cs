using MathForge.Core.Attributes;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Infinite;

[Categories("Distributions", "Univariate", "Discrete", "Infinite")]
public partial class ZetaDistribution : Distribution
{
    public override double Expected => GetExpected();
    
    public override double Mean => GetExpected();
    
    public override double Median => Quantile(0.5);
    
    public override double Mode => 1.0;
    
    public override double Variance => GetVariance();
    
    public override double Skewness => GetSkewness();
    
    public override double Kurtosis => GetKurtosis();
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    public override double Minimum => 1.0;
    
    public override double Maximum => double.PositiveInfinity;

    [EntityParameter(nameof(S))]
    public double S { get; private set; } = 2.0;

    private double _zeta;

    public override double Sample(IRandom random)
    {
        var target = random.NextDouble();
        var cumulative = 0.0;

        for (var k = 1; ; k++)
        {
            cumulative += ProbabilityMass(k);

            if (cumulative >= target)
                return k;
        }
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        var cumulative = 0.0;

        for (var k = 1; ; k++)
        {
            cumulative += ProbabilityMass(k);

            if (cumulative >= p)
                return k;
        }
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

        var maximum = (int)Math.Floor(x);
        var cumulative = 0.0;

        for (var k = 1; k <= maximum; k++)
            cumulative += ProbabilityMass(k);

        return Math.Min(cumulative, 1.0);
    }

    public double ProbabilityMass(int k)
    {
        if (k < 1)
            return 0.0;

        return 1.0 / (Math.Pow(k, S) * _zeta);
    }

    public override string ToString()
    {
        return $"Zeta Distribution [S = {S:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is ZetaDistribution other &&
               S.Equals(other.S);
    }

    public override int GetHashCode()
    {
        return S.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(S) || double.IsInfinity(S))
            throw new ArgumentException("S must be a finite number.", nameof(S));

        if (S <= 1.0)
            throw new ArgumentOutOfRangeException(nameof(S), "S must be greater than 1.");

        _zeta = RiemannZetaFunction.Calculate(S);
    }

    private double GetExpected()
    {
        if (S <= 2.0)
            return double.PositiveInfinity;

        return RiemannZetaFunction.Calculate(S - 1.0) / _zeta;
    }

    private double GetVariance()
    {
        if (S <= 3.0)
            return double.PositiveInfinity;

        var mean = Mean;

        return RiemannZetaFunction.Calculate(S - 2.0) / _zeta -
               mean * mean;
    }

    private double GetSkewness()
    {
        if (S <= 4.0)
            return double.NaN;

        var variance = Variance;

        if (double.IsInfinity(variance) || variance == 0.0)
            return double.NaN;

        var mean = Mean;
        var second = GetRawMoment(2);
        var third = GetRawMoment(3);

        var centralMoment = third
                            - 3.0 * mean * second
                            + 2.0 * Math.Pow(mean, 3.0);

        return centralMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        if (S <= 5.0)
            return double.NaN;

        var variance = Variance;

        if (double.IsInfinity(variance) || variance == 0.0)
            return double.NaN;

        var mean = Mean;
        var second = GetRawMoment(2);
        var third = GetRawMoment(3);
        var fourth = GetRawMoment(4);

        var centralMoment =
            fourth
            - 4.0 * mean * third
            + 6.0 * mean * mean * second
            - 3.0 * Math.Pow(mean, 4.0);

        return centralMoment /
               (variance * variance) - 3.0;
    }

    private double GetRawMoment(int order)
    {
        return RiemannZetaFunction.Calculate(S - order) / _zeta;
    }
    
}
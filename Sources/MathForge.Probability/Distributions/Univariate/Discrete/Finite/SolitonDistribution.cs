using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Finite;

[Categories("Distributions", "Univariate", "Discrete", "Finite")]
public partial class SolitonDistribution : Distribution
{
    public override double Expected => GetMean();

    public override double Mean => GetMean();

    public override double Median => Quantile(0.5);

    public override double Mode => GetMode();

    public override double Variance => GetVariance();

    public override double Skewness => GetSkewness();

    public override double Kurtosis => GetKurtosis();

    public override double StandardDeviation => Math.Sqrt(Variance);

    public override double Minimum => 1;

    public override double Maximum => K;

    [EntityParameter(nameof(K))]
    public int K { get; private set; } = 1;

    public override double Sample(IRandom random)
    {
        var value = random.NextDouble();
        var cumulative = 0.0;

        for (var k = 1; k <= K; k++)
        {
            cumulative += ProbabilityMass(k);

            if (value < cumulative)
                return k;
        }

        return K;
    }

    public override double Quantile(double p)
    {
        if (p < 0 || p > 1)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0)
            return Minimum;

        if (p == 1)
            return Maximum;

        var cumulative = 0.0;

        for (var k = 1; k <= K; k++)
        {
            cumulative += ProbabilityMass(k);

            if (p <= cumulative)
                return k;
        }

        return Maximum;
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < Minimum || x > Maximum)
            return 0;

        if (x != Math.Floor(x))
            return 0;

        return ProbabilityMass((int)x);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < Minimum)
            return 0;

        if (x >= Maximum)
            return 1;

        var floorX = (int)Math.Floor(x);
        var cumulative = 0.0;

        for (var k = 1; k <= floorX; k++)
            cumulative += ProbabilityMass(k);

        return cumulative;
    }

    public double ProbabilityMass(int k)
    {
        if (k < 1 || k > K)
            return 0;

        if (k == 1)
            return 1.0 / K;

        return 1.0 / (k * (k - 1));
    }

    public override string ToString()
    {
        return $"Soliton Distribution [K = {K}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is SolitonDistribution other && K == other.K;
    }

    public override int GetHashCode()
    {
        return K;
    }

    protected override void Validate()
    {
        if (K <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(K),
                "K must be greater than zero.");
        }
    }

    private double GetMean()
    {
        var mean = 0.0;

        for (var k = 1; k <= K; k++)
            mean += k * ProbabilityMass(k);

        return mean;
    }

    private double GetVariance()
    {
        var mean = Mean;
        var variance = 0.0;

        for (var k = 1; k <= K; k++)
        {
            var difference = k - mean;

            variance += difference * difference * ProbabilityMass(k);
        }

        return variance;
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var thirdMoment = 0.0;

        for (var k = 1; k <= K; k++)
        {
            var difference = k - mean;

            thirdMoment +=
                difference *
                difference *
                difference *
                ProbabilityMass(k);
        }

        return thirdMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var fourthMoment = 0.0;

        for (var k = 1; k <= K; k++)
        {
            var difference = k - mean;

            fourthMoment +=
                difference *
                difference *
                difference *
                difference *
                ProbabilityMass(k);
        }

        return fourthMoment / (variance * variance) - 3;
    }

    private double GetMode()
    {
        return K == 1 ? 1 : 2;
    }
}
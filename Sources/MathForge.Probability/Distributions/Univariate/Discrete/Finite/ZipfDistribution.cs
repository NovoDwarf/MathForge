using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Finite;

[Categories("Distributions", "Univariate", "Discrete", "Finite")]
public partial class ZipfDistribution : Distribution
{
    public override double Expected => GetMean();

    public override double Mean => GetMean();

    public override double Median => Quantile(0.5);

    public override double Mode => 1;

    public override double Variance => GetVariance();

    public override double Skewness => GetSkewness();

    public override double Kurtosis => GetKurtosis();

    public override double StandardDeviation => Math.Sqrt(Variance);

    public override double Minimum => 1;

    public override double Maximum => N;

    [EntityParameter(nameof(N))]
    public int N { get; private set; } = 10;

    [EntityParameter(nameof(S))]
    public double S { get; private set; } = 1;

    public override double Sample(IRandom random)
    {
        var value = random.NextDouble();
        var cumulative = 0.0;

        for (var k = 1; k <= N; k++)
        {
            cumulative += ProbabilityMass(k);

            if (value <= cumulative)
                return k;
        }

        return N;
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

        for (var k = 1; k <= N; k++)
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
        if (k < 1 || k > N)
            return 0;

        return Math.Pow(k, -S) / GetGeneralizedHarmonicNumber();
    }

    public override string ToString()
    {
        return $"Zipf Distribution [N = {N}, S = {S:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is ZipfDistribution other &&
               N == other.N &&
               S == other.S;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(N, S);
    }

    protected override void Validate()
    {
        if (N <= 0)
            throw new ArgumentOutOfRangeException(nameof(N), "N must be greater than zero.");

        if (double.IsNaN(S) || double.IsInfinity(S))
            throw new ArgumentException("S must be a finite number.", nameof(S));

        if (S <= 0)
            throw new ArgumentOutOfRangeException(nameof(S), "S must be greater than zero.");
    }

    private double GetMean()
    {
        var normalization = GetGeneralizedHarmonicNumber();

        var mean = 0.0;

        for (var k = 1; k <= N; k++)
            mean += Math.Pow(k, 1 - S) / normalization;

        return mean;
    }

    private double GetVariance()
    {
        var mean = Mean;
        var normalization = GetGeneralizedHarmonicNumber();
        var secondMoment = 0.0;

        for (var k = 1; k <= N; k++)
            secondMoment +=
                Math.Pow(k, 2 - S) / normalization;

        return secondMoment - mean * mean;
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var normalization = GetGeneralizedHarmonicNumber();
        var thirdMoment = 0.0;

        for (var k = 1; k <= N; k++)
        {
            var difference = k - mean;

            thirdMoment += Math.Pow(difference, 3) * Math.Pow(k, -S) / normalization;
        }

        return thirdMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var normalization = GetGeneralizedHarmonicNumber();
        var fourthMoment = 0.0;

        for (var k = 1; k <= N; k++)
        {
            var difference = k - mean;

            fourthMoment += Math.Pow(difference, 4) * Math.Pow(k, -S) / normalization;
        }

        return fourthMoment / (variance * variance) - 3;
    }

    private double GetGeneralizedHarmonicNumber()
    {
        var sum = 0.0;

        for (var k = 1; k <= N; k++)
            sum += Math.Pow(k, -S);

        return sum;
    }
}
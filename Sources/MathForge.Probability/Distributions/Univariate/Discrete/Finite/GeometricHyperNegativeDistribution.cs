using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Finite;

[Categories("Distributions", "Univariate", "Discrete", "Finite")]
public partial class GeometricHyperNegativeDistribution : Distribution
{
    public override double Expected => GetMean();

    public override double Mean => GetMean();

    public override double Median => Quantile(0.5);

    public override double Mode => GetMode();

    public override double Variance => GetVariance();

    public override double Skewness => GetSkewness();

    public override double Kurtosis => GetKurtosis();

    public override double StandardDeviation => Math.Sqrt(Variance);

    public override double Minimum => 0;

    public override double Maximum => Failures;

    [EntityParameter(nameof(Successes))]
    public int Successes { get; private set; }

    [EntityParameter(nameof(Failures))]
    public int Failures { get; private set; }

    [EntityParameter(nameof(RequiredSuccesses))]
    public int RequiredSuccesses { get; private set; } = 1;

    public int PopulationSize => Successes + Failures;
    
    public override double Sample(IRandom random)
    {
        var successes = 0;
        var failures = 0;
        var remainingSuccesses = Successes;
        var remainingFailures = Failures;

        while (successes < RequiredSuccesses)
        {
            var total = remainingSuccesses + remainingFailures;

            if (random.NextDouble() < (double)remainingSuccesses / total)
            {
                successes++;
                remainingSuccesses--;
            }
            else
            {
                failures++;
                remainingFailures--;
            }
        }

        return failures;
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

        for (var k = 0; k <= Maximum; k++)
        {
            cumulative += ProbabilityMass(k);

            if (p <= cumulative)
                return k;
        }

        return Maximum;
    }

    public override double ProbabilityDensity(double x)
    {
        var k = (int)Math.Floor(x);

        if (k < Minimum || k > Maximum || x != k)
            return 0;

        return ProbabilityMass(k);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < Minimum)
            return 0;

        if (x >= Maximum)
            return 1;

        var floorX = (int)Math.Floor(x);
        var cumulative = 0.0;

        for (var k = 0; k <= floorX; k++)
            cumulative += ProbabilityMass(k);

        return cumulative;
    }

    public double ProbabilityMass(int failures)
    {
        if (failures < 0 || failures > Failures)
            return 0;

        if (failures > Failures - RequiredSuccesses + 1 && Successes == RequiredSuccesses)
            return 0;

        var numerator = Combination(Failures, failures) * Combination(Successes, RequiredSuccesses - 1);
        var denominator = Combination(Successes + Failures, failures + RequiredSuccesses);

        return numerator / denominator;
    }
    
    public override string ToString()
    {
        return $"Negative Hypergeometric Distribution " +
               $"[Successes = {Successes}, " +
               $"Failures = {Failures}, " +
               $"RequiredSuccesses = {RequiredSuccesses}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is GeometricHyperNegativeDistribution other &&
               Successes == other.Successes &&
               Failures == other.Failures &&
               RequiredSuccesses == other.RequiredSuccesses;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Successes, Failures, RequiredSuccesses);
    }

    protected override void Validate()
    {
        if (Successes <= 0)
            throw new ArgumentOutOfRangeException(nameof(Successes), "Number of successes must be greater than zero.");

        if (Failures < 0)
            throw new ArgumentOutOfRangeException(nameof(Failures), "Number of failures cannot be negative.");

        if (RequiredSuccesses <= 0)
            throw new ArgumentOutOfRangeException(nameof(RequiredSuccesses), "Required number of successes must be greater than zero.");

        if (RequiredSuccesses > Successes)
            throw new ArgumentOutOfRangeException(nameof(RequiredSuccesses), "Required successes cannot exceed total successes.");
    }

    private double GetMean()
    {
        return (double)RequiredSuccesses * Failures /
               (Successes + 1);
    }

    private double GetVariance()
    {
        var n = Successes;
        var m = Failures;
        var r = RequiredSuccesses;

        return (double)r * m *
               (n + m + 1) *
               (n - r + 1) /
               ((n + 1) * (n + 1) * (n + 2));
    }

    private double GetMode()
    {
        var mode =
            Math.Floor(
                (double)(RequiredSuccesses - 1) *
                (Failures + 1) /
                (Successes + 2));

        return Math.Min(mode, Maximum);
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var thirdMoment = 0.0;

        for (var k = 0; k <= Maximum; k++)
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
        var mean = Mean;
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var fourthMoment = 0.0;

        for (var k = 0; k <= Maximum; k++)
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

    private static double Combination(int n, int k)
    {
        if (k < 0 || k > n)
            return 0;

        if (k == 0 || k == n)
            return 1;

        k = Math.Min(k, n - k);

        var result = 1.0;

        for (var i = 1; i <= k; i++)
            result *= (double)(n - k + i) / i;

        return result;
    }
}
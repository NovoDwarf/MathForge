using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Finite;

[Categories("Distributions", "Univariate", "Discrete", "Finite")]
public partial class RademacherDistribution : Distribution
{
    public override double Expected => Mean;

    public override double Mean => GetMean();

    public override double Median => GetMedian();

    public override double Mode => GetMode();

    public override double Variance => GetVariance();

    public override double Skewness => GetSkewness();

    public override double Kurtosis => GetKurtosis();

    public override double StandardDeviation => Math.Sqrt(Variance);

    public override double Minimum => -1;

    public override double Maximum => 1;

    [EntityParameter(nameof(Probability))]
    public double Probability { get; private set; } = 0.5;

    public override double Sample(IRandom random)
    {
        return random.NextDouble() < Probability ? 1 : -1;
    }

    public override double Quantile(double p)
    {
        if (p < 0 || p > 1)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0)
            return -1;

        if (p <= 1 - Probability)
            return -1;

        return 1;
    }

    public override double ProbabilityDensity(double x)
    {
        if (x == -1)
            return 1 - Probability;

        if (x == 1)
            return Probability;

        return 0;
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < -1)
            return 0;

        if (x < 1)
            return 1 - Probability;

        return 1;
    }

    public double ProbabilityMass(int value)
    {
        return value switch
        {
            -1 => 1 - Probability,
            1 => Probability,
            _ => 0
        };
    }

    public override string ToString()
    {
        return $"Rademacher Distribution [Probability = {Probability:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is RademacherDistribution other && Probability == other.Probability;
    }

    public override int GetHashCode()
    {
        return Probability.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(Probability) || double.IsInfinity(Probability))
            throw new ArgumentException("Probability must be a finite number.", nameof(Probability));

        if (Probability < 0 || Probability > 1)
            throw new ArgumentOutOfRangeException(nameof(Probability), "Probability must be between 0 and 1.");
    }

    private double GetMean()
    {
        return 2 * Probability - 1;
    }

    private double GetMedian()
    {
        return Probability >= 0.5
            ? 1
            : -1;
    }

    private double GetMode()
    {
        return Probability >= 0.5
            ? 1
            : -1;
    }

    private double GetVariance()
    {
        return 4 * Probability * (1 - Probability);
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        return (1 - 2 * Probability) / Math.Sqrt(Probability * (1 - Probability));
    }

    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        return (1 - 6 * Probability * (1 - Probability)) / (Probability * (1 - Probability));
    }
}
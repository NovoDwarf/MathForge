using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Infinite;

[Categories("Distributions", "Univariate", "Discrete", "Infinite")]
public partial class LogarithmicDistribution : Distribution
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

    [EntityParameter(nameof(P))]
    public double P { get; private set; } = 0.5;

    public override double Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        var u = random.NextDouble();
        var probability = ProbabilityMass(1);
        var cumulative = probability;
        var k = 1;

        while (u > cumulative)
        {
            k++;

            probability = ProbabilityMass(k);
            cumulative += probability;
        }

        return k;
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

        return -Math.Pow(P, k) /
               (k * Math.Log(1.0 - P));
    }

    public override string ToString()
    {
        return $"Logarithmic Distribution [P = {P:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is LogarithmicDistribution other && P.Equals(other.P);
    }

    public override int GetHashCode()
    {
        return P.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(P) || double.IsInfinity(P))
            throw new ArgumentException("P must be a finite number.", nameof(P));

        if (P <= 0.0 || P >= 1.0)
            throw new ArgumentOutOfRangeException(nameof(P), "P must be between 0 and 1.");
    }

    private double GetExpected()
    {
        return -P / ((1.0 - P) * Math.Log(1.0 - P));
    }

    private double GetVariance()
    {
        var log = Math.Log(1.0 - P);

        return -P * (P + log) / ((1.0 - P) * (1.0 - P) * log * log);
    }

    private double GetSkewness()
    {
        var mean = Mean;
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var thirdMoment = GetRawMoment(3);
        var centralMoment = thirdMoment - 3.0 * mean * GetRawMoment(2) + 2.0 * Math.Pow(mean, 3);

        return centralMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var mean = Mean;
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var second = GetRawMoment(2);
        var third = GetRawMoment(3);
        var fourth = GetRawMoment(4);

        var centralMoment =
            fourth
            - 4.0 * mean * third
            + 6.0 * mean * mean * second
            - 3.0 * Math.Pow(mean, 4);

        return centralMoment /
               (variance * variance) - 3.0;
    }

    private double GetRawMoment(int order)
    {
        var sum = 0.0;
        var probability = ProbabilityMass(1);
        var k = 1;

        while (probability > 1e-15)
        {
            sum += Math.Pow(k, order) * probability;

            k++;
            probability = ProbabilityMass(k);
        }

        return sum;
    }
}
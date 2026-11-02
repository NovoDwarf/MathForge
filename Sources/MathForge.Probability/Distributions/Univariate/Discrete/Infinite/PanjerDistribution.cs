using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Infinite;

[Categories("Distributions", "Univariate", "Discrete", "Infinite")]
public partial class PanjerDistribution : Distribution
{
    // TODO: performance -> ???
    
    public override double Expected => GetExpected();
    
    public override double Mean => GetExpected();
    
    public override double Median => Quantile(0.5);
    
    public override double Mode => GetMode();
    
    public override double Variance => GetVariance();
    
    public override double Skewness => GetSkewness();
    
    public override double Kurtosis => GetKurtosis();
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    
    public override double Minimum => 0.0;
    
    public override double Maximum => double.PositiveInfinity;

    [EntityParameter(nameof(A))]
    public double A { get; private set; } = 0.0;

    [EntityParameter(nameof(B))]
    public double B { get; private set; } = 1.0;

    public override double Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        var value = 0;
        var cumulative = ProbabilityMass(0);
        var target = random.NextDouble();

        while (target > cumulative)
        {
            value++;
            cumulative += ProbabilityMass(value);
        }

        return value;
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(
                nameof(p),
                "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        var cumulative = 0.0;

        for (var k = 0; ; k++)
        {
            cumulative += ProbabilityMass(k);

            if (cumulative >= p)
                return k;
        }
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < 0.0 || x != Math.Floor(x))
            return 0.0;

        return ProbabilityMass((int)x);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < 0.0)
            return 0.0;

        if (double.IsPositiveInfinity(x))
            return 1.0;

        var maximum = (int)Math.Floor(x);
        var cumulative = 0.0;

        for (var k = 0; k <= maximum; k++)
            cumulative += ProbabilityMass(k);

        return Math.Min(cumulative, 1.0);
    }

    public double ProbabilityMass(int k)
    {
        if (k < 0)
            return 0.0;

        if (k == 0)
            return GetProbabilityZero();

        var probability = GetProbabilityZero();

        for (var i = 1; i <= k; i++)
        {
            probability *= A + B / i;

            if (probability <= 0.0)
                return 0.0;
        }

        return probability;
    }

    public override string ToString()
    {
        return $"Panjer Distribution [A = {A:F3}, B = {B:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is PanjerDistribution other &&
               A.Equals(other.A) &&
               B.Equals(other.B);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(A, B);
    }

    protected override void Validate()
    {
        if (double.IsNaN(A) || double.IsInfinity(A))
            throw new ArgumentException("A must be a finite number.", nameof(A));

        if (double.IsNaN(B) || double.IsInfinity(B))
            throw new ArgumentException("B must be a finite number.", nameof(B));

        if (A >= 1.0)
            throw new ArgumentOutOfRangeException(nameof(A), "A must be less than 1.");

        if (A < 0.0 && B < 0.0)
            throw new ArgumentException("A and B cannot both be negative.");

        var p0 = GetProbabilityZero();

        if (p0 <= 0.0 || p0 > 1.0 || double.IsNaN(p0) || double.IsInfinity(p0))
            throw new ArgumentException("Parameters A and B do not define a valid Panjer distribution.");

        if (A == 0.0 && B < 0.0)
            throw new ArgumentOutOfRangeException(nameof(B), "B must be non-negative when A is zero.");

        if (A > 0.0 && B < -A)
            throw new ArgumentOutOfRangeException(nameof(B), "B must satisfy B >= -A when A is positive.");
    }

    private double GetProbabilityZero()
    {
        if (A == 0.0)
            return Math.Exp(-B);

        return Math.Pow(1.0 - A, B / A);
    }

    private double GetExpected()
    {
        if (A == 1.0)
            return double.PositiveInfinity;

        return B / (1.0 - A);
    }

    private double GetVariance()
    {
        if (A == 1.0)
            return double.PositiveInfinity;

        return B / Math.Pow(1.0 - A, 3.0);
    }

    private double GetMode()
    {
        var maximum = ProbabilityMass(0);
        var mode = 0;

        for (var k = 1; k < 10000; k++)
        {
            var probability = ProbabilityMass(k);

            if (probability > maximum)
            {
                maximum = probability;
                mode = k;
            }
            else if (probability < maximum)
            {
                break;
            }
        }

        return mode;
    }

    private double GetSkewness()
    {
        var mean = Mean;
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var thirdMoment = GetRawMoment(3);

        var centralMoment = thirdMoment
                            - 3.0 * mean * GetRawMoment(2)
                            + 2.0 * Math.Pow(mean, 3.0);

        return centralMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var mean = Mean;
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var secondMoment = GetRawMoment(2);
        var thirdMoment = GetRawMoment(3);
        var fourthMoment = GetRawMoment(4);

        var centralMoment = fourthMoment
                            - 4.0 * mean * thirdMoment
                            + 6.0 * mean * mean * secondMoment
                            - 3.0 * Math.Pow(mean, 4.0);

        return centralMoment / (variance * variance) - 3.0;
    }

    private double GetRawMoment(int order)
    {
        var sum = 0.0;

        for (var k = 0; k < 100000; k++)
        {
            var probability = ProbabilityMass(k);

            if (probability <= 1e-15)
                break;

            sum += Math.Pow(k, order) * probability;
        }

        return sum;
    }
}
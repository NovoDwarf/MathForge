using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Infinite;

[Categories("Distributions", "Univariate", "Discrete", "Infinite")]
public partial class YuleSimonDistribution : Distribution
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

    [EntityParameter(nameof(Rho))]
    public double Rho { get; private set; } = 2.0;
    
    public override double Sample(IRandom random)
    {
        var p = SampleBetaOneRho(random);
        var u = random.NextDouble();

        return Math.Floor(Math.Log(1.0 - u) / Math.Log(1.0 - p)) + 1.0;
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return double.PositiveInfinity;

        var cumulative = 0.0;
        var probability = ProbabilityMass(1);

        for (var k = 1; ; k++)
        {
            if (k > 1)
                probability *= (k - 1.0) / (k + Rho);

            cumulative += probability;

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
        var probability = ProbabilityMass(1);

        for (var k = 1; k <= maximum; k++)
        {
            if (k > 1)
            {
                probability *= (k - 1.0) / (k + Rho);
            }

            cumulative += probability;
        }

        return Math.Min(cumulative, 1.0);
    }

    public double ProbabilityMass(int k)
    {
        if (k < 1)
            return 0.0;

        if (k == 1)
            return Rho / (Rho + 1.0);

        var probability = Rho / (Rho + 1.0);

        for (var i = 2; i <= k; i++) probability *= (i - 1.0) / (i + Rho);

        return probability;
    }

    public override string ToString()
    {
        return $"Yule-Simon Distribution [Rho = {Rho:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is YuleSimonDistribution other && Rho.Equals(other.Rho);
    }

    public override int GetHashCode()
    {
        return Rho.GetHashCode();
    }

    protected override void Validate()
    {
        if (double.IsNaN(Rho) || double.IsInfinity(Rho))
            throw new ArgumentException("Rho must be a finite number.", nameof(Rho));

        if (Rho <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Rho), "Rho must be greater than 0.");
    }

    private double GetExpected()
    {
        if (Rho <= 1.0)
            return double.PositiveInfinity;

        return Rho / (Rho - 1.0);
    }

    private double GetVariance()
    {
        if (Rho <= 2.0)
            return double.PositiveInfinity;

        return Rho * Rho / ((Rho - 1.0) * (Rho - 1.0) * (Rho - 2.0));
    }

    private double GetSkewness()
    {
        if (Rho <= 3.0)
            return double.NaN;

        var variance = Variance;

        if (double.IsInfinity(variance))
            return double.NaN;

        var mean = Mean;

        var second = GetRawMoment(2);
        var third = GetRawMoment(3);

        var centralMoment =
            third
            - 3.0 * mean * second
            + 2.0 * Math.Pow(mean, 3.0);

        return centralMoment /
               Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        if (Rho <= 4.0)
            return double.NaN;

        var variance = Variance;

        if (double.IsInfinity(variance))
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

        return centralMoment / (variance * variance) - 3.0;
    }

    private double GetRawMoment(int order)
    {
        var sum = 0.0;
        var probability = ProbabilityMass(1);

        for (var k = 1; k < 1000000; k++)
        {
            if (k > 1) 
                probability *= (k - 1.0) / (k + Rho);

            sum += Math.Pow(k, order) * probability;

            if (probability < 1e-15)
                break;
        }

        return sum;
    }

    private double SampleBetaOneRho(IRandom random)
    {
        return 1.0 - Math.Pow(random.NextDouble(), 1.0 / Rho);
    }
}
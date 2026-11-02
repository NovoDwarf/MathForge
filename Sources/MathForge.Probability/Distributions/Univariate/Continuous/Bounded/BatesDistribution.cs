using MathForge.Core.Attributes;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class BatesDistribution : Distribution
{
    public override double Expected => 0.5;
    
    public override double Mean => 0.5;
    
    public override double Median => 0.5;
    
    public override double Mode => 0.5;
    
    public override double Variance => 1.0 / (12.0 * N);
    
    public override double Skewness => 0.0;
    
    public override double Kurtosis => -6.0 / (5.0 * N);
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    
    public override double Minimum => 0.0;
    
    public override double Maximum => 1.0;

    [EntityParameter(nameof(N))]
    public int N { get; private set; } = 1;

    public override double Sample(IRandom random)
    {
        var sum = 0.0;

        for (var i = 0; i < N; i++)
            sum += random.NextDouble();

        return sum / N;
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");
        
        if (p == 0.0)
            return 0.0;

        if (p == 1.0)
            return 1.0;

        var low = 0.0;
        var high = 1.0;

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
        if (x < 0.0 || x > 1.0)
            return 0.0;

        if (N == 1)
            return 1.0;

        return GetProbabilityDensity(x);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x <= 0.0)
            return 0.0;

        if (x >= 1.0)
            return 1.0;

        if (N == 1)
            return x;

        return GetCumulativeDistribution(x);
    }

    public override string ToString()
    {
        return $"Bates Distribution [N = {N}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is BatesDistribution other && N == other.N;
    }

    public override int GetHashCode()
    {
        return N;
    }

    protected override void Validate()
    {
        if (N <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(N), "N must be greater than 0.");
        }
    }

    private double GetProbabilityDensity(double x)
    {
        var upper = Math.Min(N - 1, (int)Math.Floor(N * x));
        var sum = 0.0;

        for (var k = 0; k <= upper; k++)
        {
            var value = N * x - k;

            if (value <= 0.0)
                continue;

            var coefficient = Math.Pow(-1.0, k) * BinomialCoefficient(N, k);

            sum += coefficient * Math.Pow(value, N - 1);
        }

        return N / FactorialFunction.Calculate(N - 1) * sum;
    }

    private double GetCumulativeDistribution(double x)
    {
        var upper = Math.Min(N - 1, (int)Math.Floor(N * x));
        var sum = 0.0;

        for (var k = 0; k <= upper; k++)
        {
            var value = N * x - k;

            if (value <= 0.0)
                continue;

            var coefficient = Math.Pow(-1.0, k) * BinomialCoefficient(N, k);

            sum += coefficient * Math.Pow(value, N);
        }

        return sum / FactorialFunction.Calculate(N);
    }

    private static double BinomialCoefficient(int n, int k)
    {
        if (k < 0 || k > n)
            return 0.0;

        if (k == 0 || k == n)
            return 1.0;

        k = Math.Min(k, n - k);

        var result = 1.0;

        for (var i = 1; i <= k; i++)
        {
            result *= n - k + i;
            result /= i;
        }

        return result;
    }
}
using MathForge.Core.Attributes;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class BaldingNicholsDistribution : Distribution
{
    public override double Expected => P;
    
    public override double Mean => P;
    
    public override double Median => GetMedian();
    
    public override double Mode => GetMode();
    
    public override double Variance => GetVariance();
    
    public override double Skewness => GetSkewness();
    
    public override double Kurtosis => GetKurtosis();
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    
    public override double Minimum => 0.0;
    
    public override double Maximum => 1.0;

    [EntityParameter(nameof(P))]
    public double P { get; private set; } = 0.5;

    [EntityParameter(nameof(Fst))]
    public double Fst { get; private set; } = 0.1;

    private double _alpha;
    private double _beta;

    public override double Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (Fst == 0.0)
            return P;

        return SampleBeta(random, _alpha, _beta);
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (Fst == 0.0)
            return P;

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return Maximum;

        return InverseBetaCdf(p);
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < 0.0 || x > 1.0)
            return 0.0;

        if (Fst == 0.0)
            return double.NaN;

        if (x == 0.0)
            return GetBoundaryDensity(_alpha);

        if (x == 1.0)
            return GetBoundaryDensity(_beta);

        return Math.Exp(
            (_alpha - 1.0) * Math.Log(x) +
            (_beta - 1.0) * Math.Log(1.0 - x) -
            GetLogBeta(_alpha, _beta));
    }

    public override double CumulativeDistribution(double x)
    {
        if (x <= 0.0)
            return 0.0;

        if (x >= 1.0)
            return 1.0;

        if (Fst == 0.0)
            return x < P ? 0.0 : 1.0;

        return BetaRegularizedIncompleteFunction.Calculate(x, _alpha, _beta);
    }

    public override string ToString()
    {
        return $"Balding-Nichols Distribution [P = {P:F3}, Fst = {Fst:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is BaldingNicholsDistribution other &&
               P.Equals(other.P) &&
               Fst.Equals(other.Fst);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(P, Fst);
    }

    protected override void Validate()
    {
        if (double.IsNaN(P) || double.IsInfinity(P))
            throw new ArgumentException("P must be finite.", nameof(P));

        if (P < 0.0 || P > 1.0)
            throw new ArgumentOutOfRangeException(nameof(P), "P must be between 0 and 1.");

        if (double.IsNaN(Fst) || double.IsInfinity(Fst))
            throw new ArgumentException("Fst must be finite.", nameof(Fst));

        if (Fst < 0.0 || Fst > 1.0)
            throw new ArgumentOutOfRangeException(nameof(Fst), "Fst must be between 0 and 1.");

        if (Fst == 0.0)
            return;

        var concentration = 1.0 / Fst - 1.0;

        _alpha = P * concentration;
        _beta = (1.0 - P) * concentration;

        if (_alpha <= 0.0 || _beta <= 0.0)
            throw new ArgumentException("P and Fst produce invalid Beta parameters.");
    }

    private double GetVariance()
    {
        if (Fst == 0.0)
            return 0.0;

        return P * (1.0 - P) * Fst;
    }

    private double GetMedian()
    {
        if (Fst == 0.0)
            return P;

        return InverseBetaCdf(0.5);
    }

    private double GetMode()
    {
        if (Fst == 0.0)
            return P;

        if (_alpha < 1.0 && _beta >= 1.0)
            return 0.0;

        if (_beta < 1.0 && _alpha >= 1.0)
            return 1.0;

        if (_alpha < 1.0 && _beta < 1.0)
            return double.NaN;

        return (_alpha - 1.0) / (_alpha + _beta - 2.0);
    }

    private double GetSkewness()
    {
        if (Fst == 0.0)
            return double.NaN;

        var sum = _alpha + _beta;

        return 2.0 * (_beta - _alpha) * Math.Sqrt(sum + 1.0) / ((sum + 2.0) * Math.Sqrt(_alpha * _beta));
    }

    private double GetKurtosis()
    {
        if (Fst == 0.0)
            return double.NaN;

        var sum = _alpha + _beta;

        var numerator = 6.0 *
                        (
                            Math.Pow(_alpha - _beta, 2.0) *
                            (sum + 1.0)
                            - _alpha * _beta * (sum + 2.0)
                        );

        var denominator = _alpha * _beta * (sum + 2.0) * (sum + 3.0);

        return numerator / denominator;
    }

    private static double GetBoundaryDensity(double parameter)
    {
        if (parameter < 1.0)
            return double.PositiveInfinity;

        if (parameter > 1.0)
            return 0.0;

        return 1.0;
    }

    private double GetLogBeta(double a, double b)
    {
        return LogGammaFunction.Calculate(a) +
               LogGammaFunction.Calculate(b) -
               LogGammaFunction.Calculate(a + b);
    }

    private double InverseBetaCdf(double probability)
    {
        var low = 0.0;
        var high = 1.0;

        for (var i = 0; i < 100; i++)
        {
            var middle = (low + high) / 2.0;

            if (BetaRegularizedIncompleteFunction.Calculate(middle, _alpha, _beta) < probability)
            {
                low = middle;
            }
            else
            {
                high = middle;
            }
        }

        return (low + high) / 2.0;
    }

   
    private static double SampleBeta(IRandom random, double alpha, double beta)
    {
        var x = SampleGamma(random, alpha);
        var y = SampleGamma(random, beta);

        return x / (x + y);
    }

    private static double SampleGamma(IRandom random, double shape)
    {
        if (shape < 1.0)
        {
            var u = random.NextDouble();

            return SampleGamma(random, shape + 1.0) *
                   Math.Pow(u, 1.0 / shape);
        }

        var d = shape - 1.0 / 3.0;
        var c = 1.0 / Math.Sqrt(9.0 * d);

        while (true)
        {
            var x = SampleStandardNormal(random);
            var v = 1.0 + c * x;

            if (v <= 0.0)
                continue;

            v *= v * v;

            var u = random.NextDouble();

            if (u < 1.0 - 0.0331 * x * x * x * x)
                return d * v;

            if (Math.Log(u) < 0.5 * x * x + d * (1.0 - v + Math.Log(v)))
                return d * v;
        }
    }

    private static double SampleStandardNormal(IRandom random)
    {
        var u1 = random.NextDouble();
        var u2 = random.NextDouble();

        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }
}
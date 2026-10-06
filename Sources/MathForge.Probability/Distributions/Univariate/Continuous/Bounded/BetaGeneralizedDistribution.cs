using MathForge.Core.Attributes;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Continuous.Bounded;

[Categories("Distributions", "Univariate", "Continuous", "Bounded")]
public partial class BetaGeneralizedDistribution : Distribution
{
    public override double Expected => GetExpected();
    
    public override double Mean => GetExpected();
    
    public override double Median => Quantile(0.5);
    
    public override double Mode => GetMode();
    
    public override double Variance => GetVariance();
    
    public override double Skewness => GetSkewness();
    
    public override double Kurtosis => GetKurtosis();
    
    public override double StandardDeviation => Math.Sqrt(Variance);
    
    public override double Minimum => 0.0;
    
    public override double Maximum => Scale;

    [EntityParameter(nameof(Alpha))]
    public double Alpha { get; private set; } = 2.0;

    [EntityParameter(nameof(Beta))]
    public double Beta { get; private set; } = 2.0;

    [EntityParameter(nameof(P))]
    public double P { get; private set; } = 1.0;

    [EntityParameter(nameof(Scale))]
    public double Scale { get; private set; } = 1.0;

    public override double Sample(IRandom random)
    {
        var y = SampleBeta(random, Alpha, Beta);

        return Scale * Math.Pow(y, 1.0 / P);
    }

    public override double Quantile(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0.0)
            return Minimum;

        if (p == 1.0)
            return Maximum;

        var betaQuantile = InverseRegularizedBeta(p, Alpha, Beta);

        return Scale * Math.Pow(betaQuantile, 1.0 / P);
    }

    public override double ProbabilityDensity(double x)
    {
        if (x <= 0.0 || x >= Scale)
            return 0.0;

        var y = x / Scale;

        return P /
               (Scale * BetaFunction.Calculate(Alpha, Beta)) *
               Math.Pow(y, Alpha * P - 1.0) *
               Math.Pow(1.0 - Math.Pow(y, P), Beta - 1.0);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x <= 0.0)
            return 0.0;

        if (x >= Scale)
            return 1.0;

        var y = Math.Pow(x / Scale, P);

        return BetaRegularizedIncompleteFunction.Calculate(y, Alpha, Beta);
    }

    public override string ToString()
    {
        return $"Generalized Beta Distribution [Alpha = {Alpha:F3}, Beta = {Beta:F3}, P = {P:F3}, Scale = {Scale:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is BetaGeneralizedDistribution other &&
               Alpha.Equals(other.Alpha) &&
               Beta.Equals(other.Beta) &&
               P.Equals(other.P) &&
               Scale.Equals(other.Scale);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Alpha, Beta, P, Scale);
    }

    protected override void Validate()
    {
        ValidateParameter(Alpha, nameof(Alpha));
        ValidateParameter(Beta, nameof(Beta));
        ValidateParameter(P, nameof(P));
        ValidateParameter(Scale, nameof(Scale));

        if (Alpha <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Alpha), "Alpha must be greater than 0.");

        if (Beta <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Beta), "Beta must be greater than 0.");

        if (P <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(P), "P must be greater than 0.");

        if (Scale <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Scale), "Scale must be greater than 0.");
    }

    private double GetExpected()
    {
        return Scale * Math.Exp(
                   LogGammaFunction.Calculate(Alpha + 1.0 / P) -
                   LogGammaFunction.Calculate(Alpha) +
                   LogGammaFunction.Calculate(Alpha + Beta) -
                   LogGammaFunction.Calculate(Alpha + Beta + 1.0 / P));
    }

    private double GetVariance()
    {
        var firstMoment = GetRawMoment(1);
        var secondMoment = GetRawMoment(2);

        return secondMoment - firstMoment * firstMoment;
    }

    private double GetMode()
    {
        var numerator = Alpha * P - 1.0;
        var denominator = P * (Alpha + Beta) - 1.0;

        if (numerator <= 0.0)
            return 0.0;

        if (denominator <= 0.0)
            return Scale;

        return Scale * Math.Pow(numerator / denominator, 1.0 / P);
    }

    private double GetSkewness()
    {
        var variance = Variance;

        if (variance == 0.0)
            return double.NaN;

        var mean = Mean;
        var second = GetRawMoment(2);
        var third = GetRawMoment(3);

        var centralMoment =
            third
            - 3.0 * mean * second
            + 2.0 * Math.Pow(mean, 3.0);

        return centralMoment / Math.Pow(variance, 1.5);
    }

    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0.0)
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
        return Math.Pow(Scale, order) *
               Math.Exp(
                   LogGammaFunction.Calculate(Alpha + order / P) -
                   LogGammaFunction.Calculate(Alpha) +
                   LogGammaFunction.Calculate(Alpha + Beta) -
                   LogGammaFunction.Calculate(Alpha + Beta + order / P));
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

            return SampleGamma(random, shape + 1.0) * Math.Pow(u, 1.0 / shape);
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

    private static double InverseRegularizedBeta(
        double probability,
        double alpha,
        double beta)
    {
        var low = 0.0;
        var high = 1.0;

        for (var i = 0; i < 100; i++)
        {
            var middle = (low + high) / 2.0;
            var value = BetaRegularizedIncompleteFunction.Calculate(middle, alpha, beta);

            if (value < probability)
                low = middle;
            else
                high = middle;
        }

        return (low + high) / 2.0;
    }

    private static void ValidateParameter(double value, string parameterName)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException($"{parameterName} must be finite.", parameterName);
        }
    }
}
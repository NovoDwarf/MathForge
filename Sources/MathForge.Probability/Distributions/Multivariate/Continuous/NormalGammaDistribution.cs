using MathForge.Core;
using MathForge.Core.Attributes;
using MathForge.Functions.Simple;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Multivariate.Continuous;

[Categories("Distributions", "Multivariate", "Continious")]
public partial class NormalGammaDistribution : MathEntity
{
    [EntityParameter(nameof(Mu))]
    public double Mu { get; private set; } = 0.0;

    [EntityParameter(nameof(Kappa))]
    public double Kappa { get; private set; } = 1.0;

    [EntityParameter(nameof(Alpha))]
    public double Alpha { get; private set; } = 1.0;

    [EntityParameter(nameof(Beta))]
    public double Beta { get; private set; } = 1.0;

    public double[] Mean => GetMean();

    public double[,] Covariance => GetCovariance();

    public double ProbabilityDensity(double[] x)
    {
        ValidateVector(x);

        var mu = x[0];
        var tau = x[1];

        if (tau <= 0.0)
            return 0.0;

        return Math.Exp(GetLogProbabilityDensity(mu, tau));
    }

    public double LogProbabilityDensity(double[] x)
    {
        ValidateVector(x);

        return GetLogProbabilityDensity(x[0], x[1]);
    }

    public double[] Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        var tau = SampleGamma(random, Alpha, Beta);
        var standardNormal = SampleStandardNormal(random);

        var sigma = 1.0 / Math.Sqrt(Kappa * tau);
        var mu = Mu + sigma * standardNormal;

        return [mu, tau];
    }

    public override string ToString()
    {
        return $"Normal-Gamma Distribution [Mu = {Mu:F3}, Kappa = {Kappa:F3}, Alpha = {Alpha:F3}, Beta = {Beta:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is NormalGammaDistribution other &&
               Mu.Equals(other.Mu) &&
               Kappa.Equals(other.Kappa) &&
               Alpha.Equals(other.Alpha) &&
               Beta.Equals(other.Beta);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Mu, Kappa, Alpha, Beta);
    }

    protected void Validate()
    {
        if (double.IsNaN(Mu) || double.IsInfinity(Mu))
            throw new ArgumentException("Mu must be finite.", nameof(Mu));

        if (double.IsNaN(Kappa) || double.IsInfinity(Kappa))
            throw new ArgumentException("Kappa must be finite.", nameof(Kappa));

        if (double.IsNaN(Alpha) || double.IsInfinity(Alpha))
            throw new ArgumentException("Alpha must be finite.", nameof(Alpha));

        if (double.IsNaN(Beta) || double.IsInfinity(Beta))
            throw new ArgumentException("Beta must be finite.", nameof(Beta));

        if (Kappa <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Kappa), "Kappa must be greater than 0.");

        if (Alpha <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Alpha), "Alpha must be greater than 0.");

        if (Beta <= 0.0)
            throw new ArgumentOutOfRangeException(nameof(Beta), "Beta must be greater than 0.");
    }

    private double GetLogProbabilityDensity(double mu, double tau)
    {
        if (tau <= 0.0)
            return double.NegativeInfinity;

        return
            0.5 * Math.Log(Kappa)
            + Alpha * Math.Log(Beta)
            - 0.5 * Math.Log(2.0 * Math.PI)
            - LogGammaFunction.Calculate(Alpha)
            + (Alpha - 0.5) * Math.Log(tau)
            - Beta * tau
            - 0.5 * Kappa * tau * Math.Pow(mu - Mu, 2.0);
    }

    private double[] GetMean() => [Mu, Alpha / Beta];

    private double[,] GetCovariance()
    {
        var varianceTau = Alpha / Math.Pow(Beta, 2.0);
        var varianceMu = Alpha > 1.0
            ? Beta / (Kappa * (Alpha - 1.0))
            : double.PositiveInfinity;

        return new[,]
        {
            { varianceMu, 0.0 },
            { 0.0, varianceTau }
        };
    }

    private static double SampleGamma(IRandom random, double shape, double rate)
    {
        if (shape < 1.0)
        {
            var u = random.NextDouble();

            return SampleGamma(random, shape + 1.0, rate) * Math.Pow(u, 1.0 / shape);
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
                return d * v / rate;

            if (Math.Log(u) < 0.5 * x * x + d * (1.0 - v + Math.Log(v)))
                return d * v / rate;
        }
    }

    private static double SampleStandardNormal(IRandom random)
    {
        var u1 = random.NextDouble();
        var u2 = random.NextDouble();

        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }

    private static void ValidateVector(double[] x)
    {
        if (x is null || x.Length != 2)
            throw new ArgumentException("Point must contain exactly 2 components.", nameof(x));

        if (double.IsNaN(x[0]) || double.IsInfinity(x[0]) || double.IsNaN(x[1]) || double.IsInfinity(x[1]))
            throw new ArgumentException("Point must contain finite values.", nameof(x));
    }
}
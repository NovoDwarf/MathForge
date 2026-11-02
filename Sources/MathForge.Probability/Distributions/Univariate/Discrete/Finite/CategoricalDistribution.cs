using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Univariate.Discrete.Finite;

[Categories("Distributions", "Univariate", "Discrete", "Finite")]
public partial class CategoricalDistribution : Distribution
{
    public override double Expected => Mean;

    public override double Mean => Probabilities.Select((t, i) => i * t).Sum();

    public override double Median => Quantile(0.5);

    public override double Mode => GetMode();

    public override double Variance => GetVariance();

    public override double Skewness => GetSkewness();

    public override double Kurtosis => GetKurtosis();

    public override double StandardDeviation => Math.Sqrt(Variance);

    public override double Minimum => 0;

    public override double Maximum => Probabilities.Length - 1;

    [EntityParameter(nameof(Probabilities))]
    public double[] Probabilities { get; private set; } = [];

    public int CategoryCount => Probabilities.Length;
    
    private double[] _cumulativeProbabilities = [];
    
    public override double Sample(IRandom random)
    {
        var value = random.NextDouble();

        for (var i = 0; i < _cumulativeProbabilities.Length; i++)
        {
            if (value < _cumulativeProbabilities[i])
                return i;
        }

        return Probabilities.Length - 1;
    }

    public override double Quantile(double p)
    {
        if (p < 0 || p > 1)
            throw new ArgumentOutOfRangeException(nameof(p), "Probability must be between 0 and 1.");

        if (p == 0)
            return Minimum;

        if (p == 1)
            return Maximum;

        for (var i = 0; i < _cumulativeProbabilities.Length; i++)
        {
            if (p <= _cumulativeProbabilities[i])
                return i;
        }

        return Maximum;
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < Minimum || x > Maximum)
            return 0;

        if (x != Math.Floor(x))
            return 0;

        return Probabilities[(int)x];
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < Minimum)
            return 0;

        if (x >= Maximum)
            return 1;

        return _cumulativeProbabilities[(int)Math.Floor(x)];
    }

    public double ProbabilityMass(int category)
    {
        if (category < 0 || category >= Probabilities.Length)
            return 0;

        return Probabilities[category];
    }
    
    public override string ToString()
    {
        var probabilities = string.Join(", ", Probabilities.Select(p => p.ToString("F3")));

        return $"Categorical Distribution [Probabilities = [{probabilities}]]";
    }

    public override bool Equals(object? obj)
    {
        return obj is CategoricalDistribution other && Probabilities.SequenceEqual(other.Probabilities);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var probability in Probabilities)
            hash.Add(probability);

        return hash.ToHashCode();
    }

    protected override void Validate()
    {
        if (Probabilities is null || Probabilities.Length == 0)
            throw new ArgumentException("Probabilities array cannot be null or empty.", nameof(Probabilities));

        var sum = 0.0;

        foreach (var probability in Probabilities)
        {
            if (double.IsNaN(probability) || double.IsInfinity(probability))
                throw new ArgumentException("Probabilities must be finite numbers.", nameof(Probabilities));

            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(Probabilities), "All probabilities must be between 0 and 1.");

            sum += probability;
        }

        if (Math.Abs(sum - 1.0) > 1e-12)
            throw new ArgumentException($"Probabilities must sum to 1. Actual sum: {sum}.", nameof(Probabilities));

        BuildCumulativeProbabilities();
    }

    private void BuildCumulativeProbabilities()
    {
        _cumulativeProbabilities = new double[Probabilities.Length];

        var cumulative = 0.0;

        for (var i = 0; i < Probabilities.Length; i++)
        {
            cumulative += Probabilities[i];
            _cumulativeProbabilities[i] = cumulative;
        }

        _cumulativeProbabilities[^1] = 1.0;
    }

    private double GetMode()
    {
        var mode = 0;

        for (var i = 1; i < Probabilities.Length; i++)
        {
            if (Probabilities[i] > Probabilities[mode])
                mode = i;
        }

        return mode;
    }
    
    private double GetVariance()
    {
        var mean = Mean;
        var variance = 0.0;

        for (var i = 0; i < Probabilities.Length; i++)
        {
            var difference = i - mean;
            variance += difference * difference * Probabilities[i];
        }

        return variance;
    }
    
    private double GetSkewness() 
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var thirdMoment = 0.0;

        for (var i = 0; i < Probabilities.Length; i++)
        {
            var difference = i - mean;

            thirdMoment += difference * difference * difference * Probabilities[i];
        }

        return thirdMoment / Math.Pow(variance, 1.5);
        
    }
    
    private double GetKurtosis()
    {
        var variance = Variance;

        if (variance == 0)
            return double.NaN;

        var mean = Mean;
        var fourthMoment = 0.0;

        for (var i = 0; i < Probabilities.Length; i++)
        {
            var difference = i - mean;

            fourthMoment +=
                difference * difference *
                difference * difference *
                Probabilities[i];
        }

        return fourthMoment / (variance * variance) - 3;
    }
}
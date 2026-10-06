using System.ComponentModel.DataAnnotations;
using MathForge.Core.Attributes;
using MathForge.Core.Utilities;
using MathForge.Random.Generators;
using MathForge.Vectors;

namespace MathForge.Distributions.Univariate.Continuous.Semibounded;

[Core.Attributes.Categories("Distributions", "Univariate", "Continious", "Semibounded")]
public partial class ExponentialDistribution : Distribution
{
    public override double Expected => 1.0 / Rate;
    
    public override double Mean => 1.0 / Rate;
    
    public override double Median => Math.Log(2) / Rate;
    
    public override double Mode => 0;
    
    public override double Variance => 1.0 / (Rate * Rate);
    
    public override double Skewness => 2;
    
    public override double Kurtosis => 6;
    
    public override double StandardDeviation => 1.0 / Rate;
    
    public override double Minimum => 0;
    
    public override double Maximum => double.PositiveInfinity;

    [EntityParameter(nameof(Rate))]
    [Range(0, double.PositiveInfinity)]
    public double Rate { get; private set; } = 1;

    public double Scale => 1 / Rate;
    
    public override double Sample(IRandom random)
    {
        var u = random.Next();
        
        return -Math.Log(u) / Rate;
    }
    
    public override double Quantile(double p)
    {
        MathArgumentException.ThrowIfNotInRange(p, new Range<double>(0, 1, BoundType.Exclusive, BoundType.Exclusive));

        return p switch
        {
            0 => 0,
            1 => double.PositiveInfinity,
            _ => -Math.Log(1 - p) / Rate
        };
    }

    public override double ProbabilityDensity(double x)
    {
        if (x < 0)
            return 0;

        return Rate * Math.Exp(-Rate * x);
    }

    public override double CumulativeDistribution(double x)
    {
        if (x < 0)
            return 0;

        return 1 - Math.Exp(-Rate * x);
    }
    
    public override string ToString() => $"Expo Distribution [Rate = {Rate}]";

    public override bool Equals(object? obj) => obj is ExponentialDistribution other && Rate == other.Rate;

    public override int GetHashCode() => Rate.GetHashCode();

    protected override void Validate()
    {
        
    }
}
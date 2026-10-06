using MathForge.Core;
using MathForge.Core.Attributes;
using MathForge.Random.Generators;

namespace MathForge.Distributions.Circular.Bivariate;

[Categories("Distributions", "Circular", "Bivariate")]
public partial class KentDistribution : MathEntity
{
    // TODO: rework this class to distribution
    
    public double[] Mean => (double[])_gamma1.Clone();

    public double[] Mode => (double[])_gamma1.Clone();

    public double NormalizationConstant => Math.Exp(_logNormalizationConstant);

    [EntityParameter(nameof(Kappa))]
    public double Kappa { get; private set; } = 1.0;

    [EntityParameter(nameof(Beta))]
    public double Beta { get; private set; } = 0.0;

    [EntityParameter(nameof(MeanDirection))]
    public double[] MeanDirection { get; private set; } = [1.0, 0.0, 0.0];

    [EntityParameter(nameof(MajorAxis))]
    public double[] MajorAxis { get; private set; } = [0.0, 1.0, 0.0];

    private double[] _gamma1 = [];
    private double[] _gamma2 = [];
    private double[] _gamma3 = [];

    private double _logNormalizationConstant;
    
    public double[] Sample(IRandom random)
    {
        ArgumentNullException.ThrowIfNull(random);

        while (true)
        {
            var x = SampleUniformSphere(random);
            var logAcceptance = GetLogKernel(x) - GetMaximumLogKernel();

            if (Math.Log(random.NextDouble()) <= logAcceptance)
                return x;
        }
    }
    
    public double ProbabilityDensity(double[] x)
    {
        ValidateVector(x, nameof(x));

        var logDensity = GetLogKernel(x) - _logNormalizationConstant;
        return Math.Exp(logDensity);
    }

    public double LogProbabilityDensity(double[] x)
    {
        ValidateVector(x, nameof(x));

        return GetLogKernel(x) - _logNormalizationConstant;
    }
    
    public override string ToString()
    {
        return $"Kent Distribution [Kappa = {Kappa:F3}, Beta = {Beta:F3}]";
    }

    public override bool Equals(object? obj)
    {
        return obj is KentDistribution other &&
               Kappa.Equals(other.Kappa) &&
               Beta.Equals(other.Beta) &&
               MeanDirection.SequenceEqual(other.MeanDirection) &&
               MajorAxis.SequenceEqual(other.MajorAxis);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(Kappa);
        hash.Add(Beta);

        foreach (var value in MeanDirection)
            hash.Add(value);

        foreach (var value in MajorAxis)
            hash.Add(value);

        return hash.ToHashCode();
    }

    protected void Validate()
    {
        ValidateParameters();
        BuildCoordinateSystem();

        _logNormalizationConstant = CalculateLogNormalizationConstant();
    }

    private void ValidateParameters()
    {
        if (double.IsNaN(Kappa) || double.IsInfinity(Kappa))
            throw new ArgumentException("Kappa must be finite.", nameof(Kappa));

        if (Kappa <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(Kappa),
                "Kappa must be greater than 0.");

        if (double.IsNaN(Beta) || double.IsInfinity(Beta))
            throw new ArgumentException("Beta must be finite.", nameof(Beta));

        if (Beta < 0)
            throw new ArgumentOutOfRangeException(
                nameof(Beta),
                "Beta must be greater than or equal to 0.");

        if (2.0 * Beta >= Kappa)
            throw new ArgumentOutOfRangeException(
                nameof(Beta),
                "Beta must satisfy 2 * Beta < Kappa.");

        ValidateDirection(MeanDirection, nameof(MeanDirection));
        ValidateDirection(MajorAxis, nameof(MajorAxis));

        var dot = Dot(MeanDirection, MajorAxis);

        if (Math.Abs(dot) > 1e-10)
        {
            throw new ArgumentException(
                "MajorAxis must be orthogonal to MeanDirection.",
                nameof(MajorAxis));
        }
    }

    private void BuildCoordinateSystem()
    {
        _gamma1 = Normalize(MeanDirection);
        _gamma2 = Normalize(MajorAxis);
        _gamma3 = Normalize(Cross(_gamma1, _gamma2));
    }

    private double GetLogKernel(double[] x)
    {
        var x1 = Dot(_gamma1, x);
        var x2 = Dot(_gamma2, x);
        var x3 = Dot(_gamma3, x);

        return Kappa * x1 +
               Beta * (x2 * x2 - x3 * x3);
    }

    private double GetMaximumLogKernel()
    {
        return Kappa + Beta;
    }

    private double CalculateLogNormalizationConstant()
    {
        const int intervals = 256;

        var maximum = GetMaximumLogKernel();
        var step = 2.0 / intervals;

        var sum = 0.0;

        for (var i = 0; i <= intervals; i++)
        {
            var z = -1.0 + i * step;

            var radial = betaTerm(z);
            var logValue = Kappa * z + Math.Log(ModifiedBesselI0(radial));

            var value = Math.Exp(logValue - maximum);

            var coefficient = i switch
            {
                0 => 1.0,
                intervals => 1.0,
                _ when i % 2 == 0 => 2.0,
                _ => 4.0
            };

            sum += coefficient * value;
        }

        var integral = sum * step / 3.0;

        return maximum +
               Math.Log(2.0 * Math.PI * integral);
    }

    private double betaTerm(double z)
    {
        return Beta * (1.0 - z * z);
    }

    private static double ModifiedBesselI0(double x)
    {
        var ax = Math.Abs(x);

        if (ax < 3.75)
        {
            var y = x / 3.75;
            y *= y;

            return 1.0 +
                   y * (3.5156229 +
                   y * (3.0899424 +
                   y * (1.2067492 +
                   y * (0.2659732 +
                   y * (0.0360768 +
                   y * 0.0045813)))));
        }

        var y2 = 3.75 / ax;

        return Math.Exp(ax) / Math.Sqrt(ax) *
               (0.39894228 +
                y2 * (0.01328592 +
                y2 * (0.00225319 +
                y2 * (-0.00157565 +
                y2 * (0.00916281 +
                y2 * (-0.02057706 +
                y2 * (0.02635537 +
                y2 * (-0.01647633 +
                y2 * 0.00392377))))))));
    }

    private static double[] SampleUniformSphere(IRandom random)
    {
        var z = 2.0 * random.NextDouble() - 1.0;
        var phi = 2.0 * Math.PI * random.NextDouble();

        var radius = Math.Sqrt(1.0 - z * z);

        return
        [
            radius * Math.Cos(phi),
            radius * Math.Sin(phi),
            z
        ];
    }

    private static double Dot(double[] a, double[] b)
    {
        return a[0] * b[0] +
               a[1] * b[1] +
               a[2] * b[2];
    }

    private static double[] Cross(double[] a, double[] b)
    {
        return
        [
            a[1] * b[2] - a[2] * b[1],
            a[2] * b[0] - a[0] * b[2],
            a[0] * b[1] - a[1] * b[0]
        ];
    }

    private static double[] Normalize(double[] vector)
    {
        var length = Math.Sqrt(Dot(vector, vector));

        if (length <= 0)
            throw new ArgumentException("Vector cannot have zero length.");

        return
        [
            vector[0] / length,
            vector[1] / length,
            vector[2] / length
        ];
    }

    private static void ValidateDirection(double[] vector, string parameterName)
    {
        if (vector is null || vector.Length != 3)
        {
            throw new ArgumentException(
                "Direction must contain exactly 3 components.",
                parameterName);
        }

        foreach (var value in vector)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(
                    "Direction must contain finite values.",
                    parameterName);
            }
        }

        if (Dot(vector, vector) <= 1e-20)
        {
            throw new ArgumentException(
                "Direction cannot have zero length.",
                parameterName);
        }
    }

    private static void ValidateVector(double[] vector, string parameterName)
    {
        if (vector is null || vector.Length != 3)
            throw new ArgumentException(
                "Point must contain exactly 3 components.",
                parameterName);

        var lengthSquared = Dot(vector, vector);

        if (Math.Abs(lengthSquared - 1.0) > 1e-8)
        {
            throw new ArgumentException(
                "Point must be a unit vector.",
                parameterName);
        }
    }
}
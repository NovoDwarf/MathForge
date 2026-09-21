using MathForge.Vectors.Float;

namespace MathForge.Interpolations.Polynomial;

public static class NewtonInterpolation
{
	
	// TODO: recalculating coefficients every interpolation
	
	public static double Interpolate(ReadOnlySpan<Float2> values, double x)
	{
		if (values.Length == 0)
			throw new ArgumentException("At least one point is required.", nameof(values));

		var coefficients = CalculateCoefficients(values);
		var result = coefficients[^1];

		for (var i = coefficients.Length - 2; i >= 0; i--) result = coefficients[i] + (x - values[i].X) * result;

		return result;
	}

	private static double[] CalculateCoefficients(ReadOnlySpan<Float2> values)
	{
		var count = values.Length;
		var coefficients = new double[count];

		for (var i = 0; i < count; i++)
			coefficients[i] = values[i].Y;

		for (var j = 1; j < count; j++)
		for (var i = count - 1; i >= j; i--)
		{
			coefficients[i] = (coefficients[i] - coefficients[i - 1]) / (values[i].X - values[i - j].X);
		}

		return coefficients;
	}
}
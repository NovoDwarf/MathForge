namespace MathForge.Functions.Internals.RiemannZeta;

internal static class RiemannZetaEulerMaclaurin
{
	public static double Calculate(double s)
	{
		if (s <= 1.0)
			return double.PositiveInfinity;

		const int n = 32;

		var sum = 0.0;

		for (var k = 1; k <= n; k++)
			sum += 1.0 / Math.Pow(k, s);

		var nPower = Math.Pow(n, -s);

		sum += nPower * n / (s - 1.0);
		sum += 0.5 * nPower;
		sum += s / 12.0 * Math.Pow(n, -s - 1.0);
		sum -= s * (s + 1.0) * (s + 2.0) / 720.0 * Math.Pow(n, -s - 3.0);
		sum += s * (s + 1.0) * (s + 2.0) * (s + 3.0) * (s + 4.0) / 30240.0 * Math.Pow(n, -s - 5.0);

		return sum;
	}
}
namespace MathForge.Functions.Simple;

public class BetaFunction
{
	public static double Calculate(double a, double b)
	{
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(a, 0);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(b, 0);

		return Math.Exp(LogGammaFunction.Calculate(a) + LogGammaFunction.Calculate(b) - LogGammaFunction.Calculate(a + b));
	}	
}
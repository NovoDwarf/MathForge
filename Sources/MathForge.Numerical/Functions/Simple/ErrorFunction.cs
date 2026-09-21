using MathForge.Functions.Internals.Error;

namespace MathForge.Functions.Simple;

public static class ErrorFunction
{
	public static double Calculate(double x, ErrorFunctionAlgorithm algorithm = ErrorFunctionAlgorithm.Auto)
	{
		return algorithm switch
		{
			ErrorFunctionAlgorithm.Auto => ErrorAbramowitzStegun.Calculate(x),
			ErrorFunctionAlgorithm.AbramowitzStegun => ErrorAbramowitzStegun.Calculate(x),

			_ => throw new ArgumentOutOfRangeException(nameof(algorithm))
		};
	}
}

public enum ErrorFunctionAlgorithm
{
	Auto,
	AbramowitzStegun
}
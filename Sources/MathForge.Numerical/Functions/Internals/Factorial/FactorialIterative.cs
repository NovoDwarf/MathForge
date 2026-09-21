namespace MathForge.Functions.Internals.Factorial;

public class FactorialIterative
{
	public static double Calculate(int n)
	{
		var result = 1.0;

		for (var i = 2; i <= n; i++)
			result *= i;

		return result;
	}
}
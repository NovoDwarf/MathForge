using MathForge.Maui.ViewModels.Utilities.Tools;

namespace MathForge.Maui.Systems.Algorithms.Services;

public sealed class RoundingService
{
	public double Apply(double value, RoundingMode mode)
	{
		return mode switch
		{
			RoundingMode.Up => Math.Ceiling(value),
			RoundingMode.Down => Math.Floor(value),
			RoundingMode.Nearest => Math.Round(value),
			_ => value
		};
	}
}
using System.Runtime.CompilerServices;

namespace MathForge.Interpolations.Linear;

public static class LinearInterpolation
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Interpolate(float a, float b, float t)
		=> a + (b - a) * t;
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Interpolate(double a, double b, double t)
		=> a + (b - a) * t;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static decimal Interpolate(decimal a, decimal b, decimal t)
		=> a + (b - a) * t;
}
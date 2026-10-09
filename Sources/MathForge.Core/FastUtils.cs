using System.Runtime.CompilerServices;

namespace MathForge.Graphics;

public static class FastUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Floor(double x)
	{
		var xi = (int)x;
		return x < xi ? xi - 1 : xi;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Round(double x)
	{
		return x < 0 ? (int)(x - 0.5) : (int)(x + 0.5);
	}
}
using MathForge.Vectors.Float;

namespace MathForge.Interpolations.Linear;

public static class BilinearInterpolation
{
	public static float Interpolate(
		float topLeft, float topRight, 
		float bottomLeft, float bottomRight, 
		Float2 coordinate)
	{
		var top = LinearInterpolation.Interpolate(topLeft, topRight, coordinate.X);
		var bottom = LinearInterpolation.Interpolate(bottomLeft, bottomRight, coordinate.X);

		return LinearInterpolation.Interpolate(top, bottom, coordinate.Y);
	}
}
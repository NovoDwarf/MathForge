using MathForge.Vectors.Float;

namespace MathForge.Interpolations.Spline;

public class BezierInterpolation
{
	public static Float2 Interpolate(Float2 p0, Float2 p1, Float2 p2, float t)
	{
		var a = Float2.Lerp(p0, p1, t);
		var b = Float2.Lerp(p1, p2, t);

		return Float2.Lerp(a, b, t);
	}
}
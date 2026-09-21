using MathForge.Vectors.Float;

namespace MathForge.Interpolations.Discrete;

public static class NearestNeighborInterpolation
{
	public static T Interpolate<T>(
		T topLeft, T topRight,
		T bottomLeft, T bottomRight,
		Float2 position)
	{
		var right = position.X >= 0.5f;
		var bottom = position.Y >= 0.5f;

		return (right, bottom) switch
		{
			(false, false) => topLeft,
			(true, false)  => topRight,
			(false, true)  => bottomLeft,
			(true, true)   => bottomRight
		};
	}
	
	public static T Interpolate<T>(
		T frontTopLeft, T frontTopRight,
		T frontBottomLeft, T frontBottomRight,
		T backTopLeft, T backTopRight,
		T backBottomLeft, T backBottomRight,
		Float3 position)
	{
		var right = position.X >= 0.5f;
		var bottom = position.Y >= 0.5f;
		var back = position.Z >= 0.5f;

		return (right, bottom, back) switch
		{
			(false, false, false) => frontTopLeft,
			(true,  false, false) => frontTopRight,
			(false, true,  false) => frontBottomLeft,
			(true,  true,  false) => frontBottomRight,

			(false, false, true)  => backTopLeft,
			(true,  false, true)  => backTopRight,
			(false, true,  true)  => backBottomLeft,
			(true,  true,  true)  => backBottomRight
		};
	}
}
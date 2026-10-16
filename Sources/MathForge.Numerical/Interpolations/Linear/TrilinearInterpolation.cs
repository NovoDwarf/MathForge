using MathForge.LinearAlgebra.Vectors;




using Float3 = MathForge.LinearAlgebra.Vectors.Float3;

namespace MathForge.Interpolations.Linear;

public class TrilinearInterpolation
{
	public static float Interpolate(
		float frontTopLeft, float frontTopRight, 
		float frontBottomLeft, float frontBottomRight, 
		float backTopLeft, float backTopRight, 
		float backBottomLeft, float backBottomRight, 
		Float3 coordinate)
	{
		var front = BilinearInterpolation.Interpolate(
			frontTopLeft, frontTopRight,
			frontBottomLeft, frontBottomRight, 
			(Float2)coordinate);

		var back = BilinearInterpolation.Interpolate(
			backTopLeft, backTopRight,
			backBottomLeft, backBottomRight, 
			(Float2)coordinate);

		return LinearInterpolation.Interpolate(front, back, coordinate.Z);
	}
}
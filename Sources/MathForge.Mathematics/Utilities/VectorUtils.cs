using System.Numerics;

namespace MathForge.Utilities;

/// <summary>
/// Vector math class
/// </summary>
public static class VectorUtils
{
	public static bool IsInside<T>(
		T pointX, T pointY,
		T edgePointX, T edgePointY,
		T edgeNormalX, T edgeNormalY)
		where T : INumber<T>
	{
		var vx = pointX - edgePointX;
		var vy = pointY - edgePointY;

		return Dot(vx, vy, edgeNormalX, edgeNormalY) >= T.Zero;
	}

	public static (T X, T Y) ComputeIntersection<T>(
		T ax, T ay,
		T bx, T by,
		T edgePointX, T edgePointY,
		T edgeNormalX, T edgeNormalY)
		where T : INumber<T>
	{
		var dx = bx - ax;
		var dy = by - ay;

		var ex = edgePointX - ax;
		var ey = edgePointY - ay;

		var t = Dot(ex, ey, edgeNormalX, edgeNormalY) / Dot(dx, dy, edgeNormalX, edgeNormalY);

		return (ax + t * dx, ay + t * dy);
	}
	
	#region 2D Vector Math

	public static T Dot<T>(T ax, T ay, T bx, T by) where T : INumber<T>
	{
		return ax * bx + ay * by;
	}

	public static T Cross<T>(T ax, T ay, T bx, T by) where T : INumber<T>
	{
		return ax * by - ay * bx;
	}
	
	public static T DistanceSquared<T>(T ax, T ay, T bx, T by) where T : INumber<T>
	{
		var dx = ax - bx;
		var dy = ay - by;

		return dx * dx + dy * dy;
	}
	
	#endregion
	
}
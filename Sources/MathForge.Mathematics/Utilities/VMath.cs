using System.Numerics;

namespace MathForge.Utilities;

/// <summary>
/// Vector math class
/// </summary>
public static class VMath
{
	public static T Min<T>(T a, T b) where T : INumber<T>
	{
		return T.Min(a, b);
	}

	public static T Max<T>(T a, T b) where T : INumber<T>
	{
		return T.Max(a, b);
	}

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

	#region 3D Vector Math


	public static T Dot<T>(T ax, T ay, T az, T bx, T by, T bz) where T : INumber<T>
	{
		return ax * bx + ay * by + az * bz;
	}
	
	public static T CrossX<T>(T ax, T ay, T az, T bx, T by, T bz) where T : INumber<T>
	{
		return ay * bz - az * by;
	}

	public static T CrossY<T>(T ax, T ay, T az, T bx, T by, T bz) where T : INumber<T>
	{
		return az * bx - ax * bz;
	}

	public static T CrossZ<T>(T ax, T ay, T az, T bx, T by, T bz) where T : INumber<T>
	{
		return ax * by - ay * bx;
	}
	

	
	public static T DistanceSquared<T>(T ax, T ay, T az, T bx, T by, T bz) where T : INumber<T>
	{
		var dx = ax - bx;
		var dy = ay - by;
		var dz = az - bz;

		return dx * dx + dy * dy + dz * dz;
	}

	#endregion
	
}
namespace MathForge.Geometry.Angles;

public readonly record struct Angle(double Radians)
{
	public double Degrees => Radians * (180.0 / Math.PI);

	public static Angle FromRadians(double radians) => new(radians);

	public static Angle FromDegrees(double degrees) => new(degrees * (Math.PI / 180.0));
}

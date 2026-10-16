


using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

public readonly record struct Length(double Meters);

public readonly record struct Mass(double Kilograms);

public readonly record struct Duration(double Seconds);

public readonly record struct Velocity(double MetersPerSecond);

public readonly record struct Acceleration(double MetersPerSecondSquared);

public readonly record struct Force(double Newtons);

public readonly record struct KinematicState(Double3 Position, Double3 Velocity, Double3 Acceleration);
	
public interface IIntegrator<TState>
{
	public TState Integrate(TState state, double deltaTime);
}
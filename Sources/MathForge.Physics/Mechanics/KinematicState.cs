using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>An immutable particle state: position in meters and velocity in meters per second.</summary>
public readonly record struct KinematicState
{
    public KinematicState(Double3 position, Double3 velocity)
    {
        PhysicsValidation.RequireFinite(position, nameof(position));
        PhysicsValidation.RequireFinite(velocity, nameof(velocity));
        
        Position = position;
        Velocity = velocity;
    }

    public Double3 Position { get; }
    public Double3 Velocity { get; }
}

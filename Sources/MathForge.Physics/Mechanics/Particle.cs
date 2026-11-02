using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>Immutable mass and force configuration for a material point. State is owned by the caller.</summary>
public sealed class Particle
{
    private readonly IForceModel[] _forces;

    public Particle(Mass mass, params IForceModel[] forces)
    {
        PhysicsValidation.RequirePositive(mass.Kilograms, nameof(mass));

        _forces = (IForceModel[])forces.Clone();

        foreach (var force in _forces)
        {
            if (force is null)
                throw new ArgumentException("Force models cannot contain null elements.", nameof(forces));
        }

        Mass = mass;
    }

    public Mass Mass { get; }

    public Double3 CalculateForce(double time, KinematicState state)
    {
        PhysicsValidation.RequireFinite(time, nameof(time));
        var total = Double3.Zero;

        foreach (var force in _forces)
            total += PhysicsValidation.RequireFiniteResult(force.CalculateForce(Mass, time, state));
        
        return PhysicsValidation.RequireFiniteResult(total);
    }

    public Double3 CalculateAcceleration(double time, KinematicState state)
        => PhysicsValidation.RequireFiniteResult(CalculateForce(time, state) / Mass.Kilograms);
}

using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>A drag force proportional to velocity relative to the medium.</summary>
public sealed class LinearDragForce : IForceModel
{
    /// <param name="coefficient">Drag coefficient in kilograms per second; zero disables drag.</param>
    /// <param name="mediumVelocity">Constant medium velocity in meters per second.</param>
    public LinearDragForce(double coefficient, Double3 mediumVelocity = default)
    {
        PhysicsValidation.RequireNonNegative(coefficient, nameof(coefficient));
        PhysicsValidation.RequireFinite(mediumVelocity, nameof(mediumVelocity));
        Coefficient = coefficient;
        MediumVelocity = mediumVelocity;
    }

    public double Coefficient { get; }
    public Double3 MediumVelocity { get; }

    public Double3 CalculateForce(Mass mass, double time, KinematicState state)
    {
        PhysicsValidation.ValidateEvaluation(mass, time);

        if (Coefficient == 0.0)
            return Double3.Zero;

        return PhysicsValidation.RequireFiniteResult((MediumVelocity - state.Velocity) * Coefficient);
    }
}

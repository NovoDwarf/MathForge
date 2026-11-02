using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>A Hooke spring with a fixed anchor and zero rest length.</summary>
public sealed class SpringForce : IForceModel
{
    /// <param name="stiffness">Spring stiffness in newtons per meter; zero disables the spring.</param>
    /// <param name="anchor">Fixed anchor position in meters.</param>
    public SpringForce(double stiffness, Double3 anchor = default)
    {
        PhysicsValidation.RequireNonNegative(stiffness, nameof(stiffness));
        PhysicsValidation.RequireFinite(anchor, nameof(anchor));
        Stiffness = stiffness;
        Anchor = anchor;
    }

    public double Stiffness { get; }
    public Double3 Anchor { get; }

    public Double3 CalculateForce(Mass mass, double time, KinematicState state)
    {
        PhysicsValidation.ValidateEvaluation(mass, time);

        if (Stiffness == 0.0)
            return Double3.Zero;
        
        return PhysicsValidation.RequireFiniteResult((Anchor - state.Position) * Stiffness);
    }
}

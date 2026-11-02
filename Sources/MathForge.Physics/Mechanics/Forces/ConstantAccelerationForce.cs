using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>A spatially uniform, constant acceleration field in meters per second squared.</summary>
public sealed class ConstantAccelerationForce : IForceModel
{
    public ConstantAccelerationForce(Double3 acceleration)
    {
        PhysicsValidation.RequireFinite(acceleration, nameof(acceleration));
       
        Acceleration = acceleration;
    }

    public Double3 Acceleration { get; }

    public Double3 CalculateForce(Mass mass, double time, KinematicState state)
    {
        PhysicsValidation.ValidateEvaluation(mass, time);
      
        return PhysicsValidation.RequireFiniteResult(Acceleration * mass.Kilograms);
    }
}

using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>Analytical motion under a constant acceleration, with SI units throughout.</summary>
public static class ConstantAccelerationMotion
{
    public static KinematicState Evaluate(KinematicState initialState, Double3 acceleration, Duration elapsed)
    {
        PhysicsValidation.RequireFinite(acceleration, nameof(acceleration));
        PhysicsValidation.RequireNonNegative(elapsed.Seconds, nameof(elapsed));

        if (elapsed.Seconds == 0.0)
            return initialState;
        
        var velocity = PhysicsValidation.RequireFiniteResult(initialState.Velocity + acceleration * elapsed.Seconds);
        var averageVelocity = initialState.Velocity + acceleration * (0.5 * elapsed.Seconds);
        var position = PhysicsValidation.RequireFiniteResult(initialState.Position + averageVelocity * elapsed.Seconds);

        return new KinematicState(position, velocity);
    }
}

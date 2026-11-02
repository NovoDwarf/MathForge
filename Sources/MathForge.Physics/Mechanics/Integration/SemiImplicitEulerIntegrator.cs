namespace MathForge.Physics;

/// <summary>A first-order semi-implicit Euler integrator: velocity is advanced before position.</summary>
public sealed class SemiImplicitEulerIntegrator : IIntegrator<KinematicState>
{
    private readonly Particle _particle;

    public SemiImplicitEulerIntegrator(Particle particle)
    {
        _particle = particle;
    }

    public KinematicState Integrate(KinematicState state, double time, Duration step)
    {
        PhysicsValidation.RequireFinite(time, nameof(time));
        PhysicsValidation.RequirePositive(step.Seconds, nameof(step));
        
        var endTime = time + step.Seconds;

        if (!double.IsFinite(endTime) || endTime <= time)
            throw new ArgumentOutOfRangeException(nameof(step), "The step must advance finite simulation time.");
        
        var acceleration = _particle.CalculateAcceleration(time, state);
        var velocity = PhysicsValidation.RequireFiniteResult(state.Velocity + acceleration * step.Seconds);
        var position = PhysicsValidation.RequireFiniteResult(state.Position + velocity * step.Seconds);

        return new KinematicState(position, velocity);
    }
}

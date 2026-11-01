namespace MathForge.Physics;

/// <summary>Advances a state over a finite positive step, starting at the supplied time in seconds.</summary>
/// <remarks>The integrator owns the equations to evaluate; the caller owns state and simulation time.</remarks>
public interface IIntegrator<TState>
{
    TState Integrate(TState state, double time, Duration step);
}

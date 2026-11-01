using MathForge.LinearAlgebra.Vectors;

namespace MathForge.Physics;

/// <summary>Evaluates force in newtons for the given particle mass, time in seconds, and state.</summary>
/// <remarks>Models should be deterministic and must not mutate external simulation state.</remarks>
public interface IForceModel
{
    Double3 CalculateForce(Mass mass, double time, KinematicState state);
}

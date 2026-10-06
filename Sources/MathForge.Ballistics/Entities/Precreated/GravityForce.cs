using System.Numerics;
using MathForge.Ballistics.Entities.Objects;
using MathForge.Vectors.Float;

namespace MathForge.Ballistics.Entities.Precreated;

public class GravityForce : IForce
{
	public Float3 Compute(Projectile p, World w) => w.Gravity * p.Mass;
}
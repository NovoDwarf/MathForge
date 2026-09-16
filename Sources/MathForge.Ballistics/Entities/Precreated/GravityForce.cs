using System.Numerics;
using MathForge.Ballistics.Entities.Objects;

namespace MathForge.Ballistics.Entities.Precreated;

public class GravityForce
{
	public Vector3 Compute(Projectile p, World w) => w.Gravity * p.Mass;
}
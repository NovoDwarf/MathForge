using System.Numerics;

namespace MathForge.Ballistics.Entities.Objects;

public interface IForce
{
	public Vector3 Compute(Projectile p, World w);
}

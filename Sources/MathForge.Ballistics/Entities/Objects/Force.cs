using MathForge.Vectors.Float;

namespace MathForge.Ballistics.Entities.Objects;

public interface IForce
{
	public Float3 Compute(Projectile p, World w);
}

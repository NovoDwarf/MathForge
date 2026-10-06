using MathForge.Vectors.Float;

namespace MathForge.Ballistics.Entities.Objects;

public class Projectile
{
	public float Mass { get; }
	public Float3 Position { get; set; }
	public Float3 Velocity { get; set; }

	private readonly List<IForce> _forces = [];

	public Projectile(float mass)
	{
		Mass = mass;
	}

	public void AddForce(IForce force)
		=> _forces.Add(force);

	public void Step(float dt, World w)
	{ 
		var totalForce = _forces.Aggregate(Float3.Zero, (current, f) => current + f.Compute(this, w));
		var acceleration = totalForce * (1.0f / Mass);

		Velocity += acceleration * dt;
		Position += Velocity * dt;
	}
}
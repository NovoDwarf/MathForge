using System.Numerics;
using MathForge.Ballistics.Entities.Data;
using MathForge.Vectors.Float;

namespace MathForge.Ballistics.Entities.Objects;

public class World
{
	public World(WorldData data)
	{
		Gravity = data.Gravity;
	}
	
	public Float3 Gravity { get; set; }
	public float AirDensity { get; set; }
}
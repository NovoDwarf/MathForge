using System.Numerics;
using MathForge.Ballistics.Entities.Data;

namespace MathForge.Ballistics.Entities.Objects;

public class World
{
	public World(WorldData data)
	{
		Gravity = data.Gravity;
	}
	
	public Vector3 Gravity { get; set; }
	public float AirDensity { get; set; }
}
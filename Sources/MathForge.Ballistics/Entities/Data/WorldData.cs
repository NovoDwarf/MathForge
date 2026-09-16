using System.Numerics;

namespace MathForge.Ballistics.Entities.Data;

public class WorldData
{
	public Vector3 Gravity { get; set; } = new Vector3(0, -9.81f, 0);
}